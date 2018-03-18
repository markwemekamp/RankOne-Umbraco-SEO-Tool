using System.Collections.Generic;
using System.Xml.Serialization;

namespace RankOne.Models.Settings
{
    [XmlType("Summary")]
    public class SummarySettings
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "Alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        public List<AnalyzerSettings> Analyzers { get; set; }
    }
}