using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Html
{
    [AnalyzerCategory(SummaryName = "Html")]
    public class AnchorTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "anchorTagAnalyzer"
            };

            var anchorTags = HtmlHelper.GetElements(document, "a");
            var anchorTagCount = anchorTags.Count();

            var anchorWithTitleTagCount = anchorTags.Count(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (anchorTagCount > anchorWithTitleTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "anchorTagAnalyzer_missing_title_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingTitle = anchorTagCount - anchorWithTitleTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingTitle.ToString());
                result.ResultRules.Add(resultRule);
            }
            else
            {
                result.AddResultRule("anchorTagAnalyzer_all_title_tags_present", ResultType.Hint);
            }

            return result;
        }
    }
}
