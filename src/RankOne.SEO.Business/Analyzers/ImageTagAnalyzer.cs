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

            var imagesWithAltTags = imageTags.Where(x => HtmlHelper.GetAttribute(x, "alt") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "alt").Value));

            if (imageTags.Count() > imagesWithAltTags.Count())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("missing alt tags"), Type = ResultType.Warning});
            }

            var imagesWithTitleTags = imageTags.Where(x => HtmlHelper.GetAttribute(x, "title") != null && !string.IsNullOrWhiteSpace(HtmlHelper.GetAttribute(x, "title").Value));

            if (imageTags.Count() > imagesWithTitleTags.Count())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("missing title tags"), Type = ResultType.Hint });
            }

            return result;
        }
    }
}
