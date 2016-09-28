using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordheadinganalyzer")]
    public class KeywordHeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordheadinganalyzer"
            };

            var h1Tags = HtmlHelper.GetElements(document, "h1");
            var h2Tags = HtmlHelper.GetElements(document, "h2");
            var h3Tags = HtmlHelper.GetElements(document, "h3");
            var h4Tags = HtmlHelper.GetElements(document, "h4");

            var usedInHeadingCount = h1Tags.Count(x => x.InnerText.ToLower().Contains(focuskeyword)) + 
                h2Tags.Count(x => x.InnerText.ToLower().Contains(focuskeyword)) + 
                h3Tags.Count(x => x.InnerText.ToLower().Contains(focuskeyword)) + 
                h4Tags.Count(x => x.InnerText.ToLower().Contains(focuskeyword));

            if (usedInHeadingCount > 0)
            {
                var resultRule = new ResultRule
                {
                    Alias = "keywordheadinganalyzer_keyword_used_in_heading",
                    Type = ResultType.Success
                };
                resultRule.Tokens.Add(usedInHeadingCount.ToString());
                result.ResultRules.Add(resultRule);
            }
            else
            {
                result.AddResultRule("keywordheadinganalyzer_keyword_not_used_in_heading", ResultType.Hint);
            }

            

            return result;
        }
    }
}
