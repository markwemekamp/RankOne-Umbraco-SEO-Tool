using System.Xml.Serialization;

namespace RankOne.Models.Settings
{
    [XmlType("Option")]
    public class AnalyzerOption
    {
        [XmlAttribute(AttributeName = "Key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }
}