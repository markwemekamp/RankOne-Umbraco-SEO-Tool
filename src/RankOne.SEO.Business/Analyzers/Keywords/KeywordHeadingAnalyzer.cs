using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Keywords
{
    public class KeywordHeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordheadinganalyzer"
            };

            var h1Tags = HtmlHelper.GetElements(document, "h1");
            var h2Tags = HtmlHelper.GetElements(document, "h2");
            var h3Tags = HtmlHelper.GetElements(document, "h3");
            var h4Tags = HtmlHelper.GetElements(document, "h4");
            var keyword = additionalValues[0].ToString();

            bool usedInHeading = h1Tags.Any(x => x.InnerText.ToLower().Contains(keyword)) || 
                h2Tags.Any(x => x.InnerText.ToLower().Contains(keyword)) || 
                h3Tags.Any(x => x.InnerText.ToLower().Contains(keyword)) || 
                h4Tags.Any(x => x.InnerText.ToLower().Contains(keyword));

            if (usedInHeading)
            {
                result.AddResultRule("keywordheadinganalyzer_keyword_used_in_heading", ResultType.Success);
            }
            else
            {
                result.AddResultRule("keywordheadinganalyzer_keyword_not_used_in_heading", ResultType.Hint);
            }

            return result;
        }
    }
}
