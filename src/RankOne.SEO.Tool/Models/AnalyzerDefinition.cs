using System;
using RankOne.Attributes;

namespace RankOne.Models
{
    public class AnalyzerDefinition
    {
        public Type Type { get; set; }
        public AnalyzerCategory AnalyzerCategory { get; set; }
    }
}
