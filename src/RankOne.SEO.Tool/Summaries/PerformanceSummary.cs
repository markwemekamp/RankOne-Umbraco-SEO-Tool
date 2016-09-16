using RankOne.Attributes;
using RankOne.Models;

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