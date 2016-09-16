using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Summaries
{
    [Summary(SortOrder = 3)]
    public class PerformanceSummary : BaseSummary
    {
        public PerformanceSummary(HtmlResult htmlResult) : base(htmlResult)
        {
            Name = "Performance";
        }
    }
}