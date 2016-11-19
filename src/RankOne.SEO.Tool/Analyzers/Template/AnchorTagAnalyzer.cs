using System.Linq;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "anchorTagAnalyzer")]
    public class AnchorTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var anchorTags = pageData.Document.GetDescendingElements("a");
            var anchorTagCount = anchorTags.Count();

            var anchorWithTitleTagCount = anchorTags.Count(x => x.GetAttribute("title") != null && !string.IsNullOrWhiteSpace(x.GetAttribute("title").Value));

            if (anchorTagCount > anchorWithTitleTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "missing_title_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingTitle = anchorTagCount - anchorWithTitleTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingTitle.ToString());
                result.ResultRules.Add(resultRule);
            }
            else
            {
                result.AddResultRule("all_title_tags_present", ResultType.Hint);
            }

            return result;
        }
    }
}
