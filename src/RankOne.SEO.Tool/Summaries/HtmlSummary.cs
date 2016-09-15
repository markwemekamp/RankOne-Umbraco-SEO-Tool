using RankOne.Models;

namespace RankOne.Summaries
{
    public class HtmlSummary : BaseSummary
    {
        public HtmlSummary(HtmlResult htmlResult) : base(htmlResult)
        {
            Name = "Html";
        }
    }
}
