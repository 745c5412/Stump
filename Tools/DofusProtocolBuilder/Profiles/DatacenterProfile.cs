using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;

namespace DofusProtocolBuilder.Profiles
{
    public class DatacenterProfile : ParsingProfile
    {
        public DatacenterProfile()
        {
            IgnoredLines = new string[]
                {
                    "^switch.*",
                    "^return.*",
                };

            BeforeParsingReplacementRules =
                new SerializableDictionary<string, string>
                    {
                        {@"(var\s|this\.)base(?![\w\d])", @"$1@base"},
                        // add '@' on variable name that are keyword in c#,
                        {@"(var\s|this\.)object(?![\w\d])", @"$1@object"},
                        {@"(var\s|this\.)operator(?![\w\d])", @"$1@operator"},
                        {@"(var\s|this\.)params(?![\w\d])", @"$1@params"},
                        // operator to @operator because it's a keyword

                        {@"Vector\.([\w_\d]+) = new ([\w_\d]+)();", "$1 = new List<$2>();"},
                        // convert "Vector." to List (C#) (and its props)
                        {@"new Vector\.<([\d\w]+)>\((\d+), (true|false)\)", "new List<$1>($2)"},
                        {@"new Vector\.<([\d\w]+)>", "new List<$1>()"},
                        {@"(__AS3__\.vec\.)?Vector\.", "List"},
                        {@"\.push\(", @".Add("},
                        {@"\.length", @".Count"},
                        {@"\bNumber", @"double"},
                        // convert Number to float

                        {@"static const", "const"},
                        // manual fix
                        {
                            @"const OPERATORS_LIST:Array\s?=\s?\[([^\]]+)\]",
                            "readonly static OPERATORS_LIST:Array=new string[]{$1}"
                            },
                        //another hack
                        {@"(protected|private) var _rawZone:", "public var rawZone:"},
                        {@"(protected|private) var _zoneSize = 4.29497e+009", "public var zoneSize"},
                        {@"(protected|private) var _zoneShape = 4.29497e+009", "public var zoneShape"},
                        {@"(protected|private) var _zoneMinSize = 4.29497e+009", "public var zoneMinSize"},
                        {@"public var zoneSize:Object", "public var zoneSize:uint"},
                        {@"public var zoneMinSize:Object", "public var zoneMinSize:uint"},
                        {@"public var zoneEfficiencyPercent:Object", "public var zoneEfficiencyPercent:int"},
                        {@"public var zoneMaxEfficiency:Object", "public var zoneMaxEfficiency:int"},
                        {@"public var zoneStopAtTarget:Object", "public var zoneStopAtTarget:uint"},
                        {@"(protected|private) var _weight", "public var weight"},
                        {@"public function get oldValue\(\) : int", "public var oldValue:int;"},
                        {@"public function get newValue\(\) : int", "public var newValue:int;"},
                        {@"public function get lang\(\) : String", "public var lang:String;"},
                        {@"public function get type\(\) : int", "public var type:int;"},
                        // ankama's devs are idiots, they attempt to assign -1 to a uint field
                        {@"public var iconId:uint", "public var iconId:int"},

                        // shouldn't be public
                        {@"public var type:QuestObjectiveType", "private var type:QuestObjectiveType"},

                        // Hint.as
                        {"private var _categoryId:uint;", "public var categoryId:uint;" },
                    };
        }

        public override void ExecuteProfile(Parser parser)
        {
            string file = Path.Combine(Program.Configuration.Output, OutPutPath, GetRelativePath(parser.Filename),
                                       Path.GetFileNameWithoutExtension(parser.Filename));
            
            var engine = new Engine();
            var host = new TemplateHost(TemplatePath);
            host.Session["Parser"] = parser;
            host.Session["Profile"] = this;
            string output = engine.ProcessTemplate(File.ReadAllText(TemplatePath), host);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine("File:{0} Line:{1} : {2}", error.FileName, error.Line, error.ErrorText);
            }

            if (host.Errors.Count > 0)
                Program.Shutdown();

            File.WriteAllText(file + host.FileExtension, output);

            Console.WriteLine("Wrote {0}", file + host.FileExtension);
        }
    }
}