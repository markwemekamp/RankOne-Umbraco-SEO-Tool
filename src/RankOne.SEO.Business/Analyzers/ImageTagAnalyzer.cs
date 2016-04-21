using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult();
            result.Alias = "imagetaganalyzer";

            var imageTags = HtmlHelper.GetElements(document, "img");
            var imageTagCount = imageTags.Count();

            var imagesWithAltTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "alt") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "alt").Value));

            if (imageTagCount > imagesWithAltTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = "imagetaganalyzer_missing_alt_tags", Type = ResultType.Warning });
            }

            var imagesWithTitleTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (imageTagCount > imagesWithTitleTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = "imagetaganalyzer_missing_title_tags", Type = ResultType.Hint });
            }

            if (imageTagCount == imagesWithAltTagCount && imageTagCount > imagesWithTitleTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = "imagetaganalyzer_alt_and_title_tags_present", Type = ResultType.Success });
            }

            return result;
        }
    }
}
