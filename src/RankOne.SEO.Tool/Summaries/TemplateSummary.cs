using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Summaries
{
    [Summary(SortOrder = 1)]
    public class TemplateSummary : BaseSummary
    {
        public TemplateSummary(HtmlResult htmlResult) : base(htmlResult)
        {
            Name = "Template";
        }
    }
}
