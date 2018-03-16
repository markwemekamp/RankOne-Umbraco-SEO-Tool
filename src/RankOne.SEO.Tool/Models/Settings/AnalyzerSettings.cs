using System;
using System.Collections.Generic;
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

        [XmlAttribute(AttributeName = "Weight")]
        public string WeightValue
        {
            get
            {
                if (Weight.HasValue)
                {
                    return Weight.Value.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        Weight = int.Parse(value);
                    }
                    catch (Exception)
                    {
                        Weight = 0;
                    }
                }
                else
                {
                    Weight = null;
                }
            }
        }

        public int? Weight
        {
            get; set;
        }

        public List<AnalyzerOption> Options { get; set; }
    }
}