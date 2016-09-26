using RankOne.Attributes;

namespace RankOne.Summaries
{
    [Summary(Alias = "performanceanalyzer", SortOrder = 3)]
    public class PerformanceSummary : BaseSummary
    {
        public PerformanceSummary()
        {
            Name = "Performance";
        }
    }
}