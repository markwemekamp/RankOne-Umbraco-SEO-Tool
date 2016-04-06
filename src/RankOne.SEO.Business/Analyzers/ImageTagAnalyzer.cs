using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public ImageTagAnalyzer()
        {
            Alias = "imagetaganalyzer";
        }

        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = TitleTag;

            var imageTags = HtmlHelper.GetElements(document, "img");
            var imageTagCount = imageTags.Count();

            var imagesWithAltTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "alt") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "alt").Value));

            if (imageTagCount > imagesWithAltTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("missing alt tags"), Type = ResultType.Warning });
            }

            var imagesWithTitleTagCount = imageTags.Count(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (imageTagCount > imagesWithTitleTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("missing title tags"), Type = ResultType.Hint });
            }

            if (imageTagCount == imagesWithAltTagCount && imageTagCount > imagesWithTitleTagCount)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("alt and title tags present"), Type = ResultType.Succes });
            }

            return result;
        }
    }
}
