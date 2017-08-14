using RankOne.Attributes;
using System;

namespace RankOne.Models
{
    public class AnalyzerDefinition
    {
        public Type Type { get; set; }
        public AnalyzerCategory AnalyzerCategory { get; set; }
    }
}