using System;

namespace RankOne.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnalyzerCategory : Attribute
    {
        public string Alias { get; set; }
        public string SummaryName { get; set; }
    }
}