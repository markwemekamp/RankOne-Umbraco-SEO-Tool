using System.Linq;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordheadinganalyzer")]
    public class KeywordHeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(PageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordheadinganalyzer"
            };

            var h1Tags = HtmlHelper.GetElements(pageData.Document, "h1");
            var h2Tags = HtmlHelper.GetElements(pageData.Document, "h2");
            var h3Tags = HtmlHelper.GetElements(pageData.Document, "h3");
            var h4Tags = HtmlHelper.GetElements(pageData.Document, "h4");

            var usedInHeadingCount = h1Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h2Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h3Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h4Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword));

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
