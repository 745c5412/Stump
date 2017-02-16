using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    public class ForStatement : ControlStatement
    {
        public static string Pattern =
            @"^[^;]+;\s(?<iterator>[^\s]+)[^(_\w\)]+(?<iterated>[^;]+);.*$";



        public string Iterated
        {
            get;
            set;
        }

        public string Iterator
        {
            get;
            set;
        }

        protected override void OnContentUpdated(string content)
        {
            var match = Regex.Match(content, Pattern, RegexOptions.Compiled);
            if (match.Success)
            {
                Iterated = match.Groups["iterated"].Value;
                Iterator = match.Groups["iterator"].Value;
            }
            base.OnContentUpdated(content);
        }
    }
}