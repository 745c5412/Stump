using DofusProtocolBuilder.Parsing;
using System.Xml;

namespace DofusProtocolBuilder.XmlPatterns
{
    public abstract class XmlPatternBuilder
    {
        protected Parser Parser;

        protected XmlPatternBuilder(Parser parser)
        {
            Parser = parser;
        }

        public abstract void WriteToXml(XmlWriter writer);
    }
}