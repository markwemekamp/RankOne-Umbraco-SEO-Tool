using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
{
    public class AnchorTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
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
                result.AddResultRule("anchorTagAnalyzer_missing_title_tags", ResultType.Hint);
            }
            else
            {
                result.AddResultRule("anchorTagAnalyzer_all_title_tags_present", ResultType.Hint);
            }

            return result;
        }
    }
}
