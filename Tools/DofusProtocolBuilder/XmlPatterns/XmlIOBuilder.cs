using System.Collections.Generic;
using System.Linq;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Parsing.Elements;

namespace DofusProtocolBuilder.XmlPatterns
{
    public abstract class XmlIOBuilder : XmlPatternBuilder
    {
        private const string RegexNewObject = @"new ([\w\d]+\.)*([\w\d]+)";

        protected XmlIOBuilder(Parser parser)
            : base(parser)
        {
        }

        public List<XmlField> GetXmlFields()
        {
            var xmlFields = new List<XmlField>();
            var localFields = new Dictionary<string, string>();

            var serializeMethod = Parser.Methods.Find(entry => entry.Name.Contains("serializeAs"));
            var controlsStatements = new Stack<ControlStatement>();
            var ifStatements = new Stack<ControlStatement>();
            var ignoredFields = new List<string>();

            for (var i = 0; i < serializeMethod.Statements.Count; i++)
            {
                string GetFieldCondition() => ifStatements.Count > 0 ? ifStatements.Pop().Content : null;

                bool IsArray(out string lengthOrType)
                {
                    string GetArrayType(string comparand)
                    {
                        var invoke = serializeMethod.Statements.OfType<InvokeExpression>().FirstOrDefault(x => x.Args.Length > 0 && x.Args[0].Contains(comparand));
                        return invoke.Name.Substring("Write".Length).ToLower();
                    }

                    var whileStmt = controlsStatements.FirstOrDefault(x => x.ControlType == ControlType.While);
                    if (whileStmt != null)
                    {
                        var comparand = whileStmt.Content.Split('<')[1].Trim();

                        lengthOrType = int.TryParse(comparand, out int length) ? length.ToString() : GetArrayType(comparand);
                        return true;
                    }

                    var forStmt = controlsStatements.OfType<ForStatement>().FirstOrDefault();
                    if (forStmt != null)
                    {
                        lengthOrType = int.TryParse(forStmt.Iterated, out int length) ? length.ToString() : GetArrayType(forStmt.Iterated);
                        return true;
                    }

                    lengthOrType = null;
                    return false;
                }

                void AddField(string fieldName, string fieldType)
                {
                    if (fieldName.Contains(" as "))
                        fieldName = fieldName.Substring(0, fieldName.IndexOf(" as ")).TrimStart('(');

                    if (fieldName.Contains("[")) // array, discard the part inside brackets
                        fieldName = fieldName.Substring(0, fieldName.IndexOf("["));
                 
                    if (ignoredFields.Contains(fieldName))
                        return;

                    string arrayLength;
                    if (IsArray(out arrayLength))
                        fieldType += "[]";

                    var field = Parser.Fields.Find(entry => entry.Name == fieldName);

                    if (field != null)
                        xmlFields.Add(new XmlField
                        {
                            Name = field.Name,
                            Type = fieldType,
                            ArrayLength = arrayLength,
                            Condition = GetFieldCondition()
                        });
                }

                switch (serializeMethod.Statements[i])
                {
                    case ControlStatement statement:
                        controlsStatements.Push(statement);
                        if (statement.ControlType == ControlType.If)
                            ifStatements.Push(statement);
                        break;

                    case ControlStatementEnd statement:
                        controlsStatements.Pop();
                        break;

                    case InvokeExpression statement when statement.Name.StartsWith("Write"):
                    {
                        var type = statement.Name.Substring("Write".Length).ToLower();
                        var name = statement.Args[0];

                        if (name.Contains(".Count") || name.Contains("getTypeId()")) // ignore this field
                            continue;

                        if (type == "bytes")
                            type = "sbyte[]";
						if (type == "byte")
							type = "sbyte";

                        AddField(name, type);
                    }
                        break;
                    case InvokeExpression statement when statement.Name.StartsWith("serializeAs_"): // serialize with known type
                    {
                        var type = statement.Name.Substring("serializeAs_".Length);
                        var name = statement.Target;

                        AddField(name, "Types." + type);
                    }
                        break;

                    case InvokeExpression statement when statement.Name == "serialize" && statement.Target.Contains(" as "): // serialize with unknown type
                    {
                        // (this.actors[_i4] as GameRolePlayActorInformations).serialize(output);
                        var type = statement.Target.Substring(statement.Target.IndexOf(" as ") + " as ".Length).TrimEnd(')'); // 
                        var name = statement.Target.Substring(0, statement.Target.IndexOf(" as ")).TrimStart('(');

                        AddField(name, "instance of " + type);
                    }
                        break;
                    case InvokeExpression statement when statement.Name == "serialize" && !statement.Target.Contains(" as "): // serialize with unknown type
                    {
                        // effect.serialize(output);
                        var name = statement.Target;
                        var type = Parser.Fields.First(x => x.Name == name).Type.Name;

                        AddField(name, "instance of " + type);
                    }
                        break;
                    case AssignationStatement statement when statement.Value.Contains("setFlag("):
                    {
                        var invokeExpression = InvokeExpression.Parse(statement.Value + ";");

                        var type = $"flag({invokeExpression.Args[1]})";
                        var name = invokeExpression.Args[2];

                        ignoredFields.Add(invokeExpression.Args[0]);
                        AddField(name, type);
                    }
                        break;
                }
            }

            return xmlFields;
        }
    }
}