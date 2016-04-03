using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = "imagetaganalyzer_title";

            var imageTags = HtmlHelper.GetElements(document, "img");

            var imagesWithAltTags = imageTags.Where(x => HtmlHelper.GetAttribute(x, "alt") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "alt").Value));

            if (imageTags.Count() > imagesWithAltTags.Count())
            {
                result.ResultRules.Add(new ResultRule { Code = "imagetaganalyzer_missing_alt_tags", Type = ResultType.Warning});
            }

            var imagesWithTitleTags = imageTags.Where(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (imageTags.Count() > imagesWithTitleTags.Count())
            {
                result.ResultRules.Add(new ResultRule { Code = "imagetaganalyzer_missing_title_tags", Type = ResultType.Hint });
            }

            return result;
        }
    }
}
