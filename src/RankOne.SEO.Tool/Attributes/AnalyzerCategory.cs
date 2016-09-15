using System;

namespace RankOne.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnalyzerCategory : Attribute
    {
        public string SummaryName { get; set; }
    }
}
