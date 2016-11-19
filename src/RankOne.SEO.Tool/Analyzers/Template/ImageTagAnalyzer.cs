using System.Linq;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "imagetaganalyzer")]
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var imageTags = pageData.Document.GetDescendingElements("img");
            var imageTagCount = imageTags.Count();

            var imagesWithAltTagCount = imageTags.Count(x => x.GetAttribute("alt") != null && !string.IsNullOrWhiteSpace(x.GetAttribute("alt").Value));

            if (imageTagCount > imagesWithAltTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "missing_alt_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingAlt = imageTagCount - imagesWithAltTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingAlt.ToString());
                result.ResultRules.Add(resultRule);
            }

            var imagesWithTitleTagCount = imageTags.Count(x => x.GetAttribute("title") != null && !string.IsNullOrWhiteSpace(x.GetAttribute("title").Value));

            if (imageTagCount > imagesWithTitleTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "missing_title_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingTitle = imageTagCount - imagesWithTitleTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingTitle.ToString());
                result.ResultRules.Add(resultRule);
            }

            if (imageTagCount == imagesWithAltTagCount && imageTagCount == imagesWithTitleTagCount)
            {
                result.AddResultRule("alt_and_title_tags_present", ResultType.Success);
            }

            return result;
        }
    }
}
