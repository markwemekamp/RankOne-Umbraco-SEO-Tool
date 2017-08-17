using System.Xml.Serialization;

namespace RankOne.Models.Settings
{
    [XmlType("Analyzer")]
    public class AnalyzerSettings
    {
        [XmlAttribute(AttributeName = "Alias")]
        public string Alias { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
    }
}
