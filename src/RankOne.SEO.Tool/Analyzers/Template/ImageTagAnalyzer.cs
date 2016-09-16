using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template")]
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "imagetaganalyzer"
            };

            var imageTags = HtmlHelper.GetElements(document, "img");
            var imageTagCount = imageTags.Count();

            var imagesWithAltTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "alt") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "alt").Value));

            if (imageTagCount > imagesWithAltTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "imagetaganalyzer_missing_alt_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingAlt = imageTagCount - imagesWithAltTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingAlt.ToString());
                result.ResultRules.Add(resultRule);
            }

            var imagesWithTitleTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (imageTagCount > imagesWithTitleTagCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "imagetaganalyzer_missing_title_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingTitle = imageTagCount - imagesWithTitleTagCount;
                resultRule.Tokens.Add(numberOfTagsMissingTitle.ToString());
                result.ResultRules.Add(resultRule);
            }

            if (imageTagCount == imagesWithAltTagCount && imageTagCount == imagesWithTitleTagCount)
            {
                result.AddResultRule("imagetaganalyzer_alt_and_title_tags_present", ResultType.Success);
            }

            return result;
        }
    }
}
