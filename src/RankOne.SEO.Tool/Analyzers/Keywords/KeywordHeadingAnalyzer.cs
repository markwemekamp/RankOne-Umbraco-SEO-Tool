using System.Linq;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordheadinganalyzer")]
    public class KeywordHeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            // Check for h1, h2, h3 and h4
            var headerTagCount = 0;
            for (var i = 1; i <=4;i++)
            {
                var headerTag = pageData.Document.GetElements("h" + i);
                headerTagCount += headerTag.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword));
            }

            if (headerTagCount > 0)
            {
                var resultRule = new ResultRule
                {
                    Alias = "keyword_used_in_heading",
                    Type = ResultType.Success
                };
                resultRule.Tokens.Add(headerTagCount.ToString());
                result.ResultRules.Add(resultRule);
            }
            else
            {
                result.AddResultRule("keyword_not_used_in_heading", ResultType.Hint);
            }

            return result;
        }
    }
}
