using RankOne.Models;

namespace RankOne.Summaries
{
    public class PerformanceSummary : BaseSummary
    {
        public PerformanceSummary(HtmlResult htmlResult) : base(htmlResult)
        {
            Name = "Speed";
        }
    }
}