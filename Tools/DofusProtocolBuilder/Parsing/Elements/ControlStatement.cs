using System;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    public class ControlStatementEnd : IStatement
    {
    }

    public class ControlStatement : IStatement
    {
        public static string Pattern =
            @"\b(((?<type>if|else if|else|while|for each|for)\()\s?(?<condition>.*(?=\)))?|(?<type2>break|continue))";

        private string m_content;

        public ControlType ControlType
        {
            get;
            set;
        }

        public string Content
        {
            get { return m_content; }
            set { m_content = value;
                OnContentUpdated(value);
            }
        }

        protected virtual void OnContentUpdated(string content)
        {
        }

        public static ControlStatement Parse(string str)
        {
            var match = Regex.Match(str, Pattern, RegexOptions.Compiled);
            ControlStatement result = null;

            if (match.Success)
            {
                var type = (match.Groups["type"].Success ? match.Groups["type"].Value : match.Groups["type2"].Value).Replace(" ", "");
                var condition = match.Groups["condition"].Value.Trim();

                if (type == "for")
                    result = new ForStatement();
                else
                    result = new ControlStatement();

                if (match.Groups["type"].Value != "")
                    result.ControlType = (ControlType) Enum.Parse(typeof(ControlType), type, true);

                if (match.Groups["condition"].Value != "")
                    result.Content = condition;
            }

            return result;
        }
    }
}