using System;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordurlanalyzer")]
    public class KeywordUrlAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var url = new Uri(pageData.Url);

            if (url.AbsolutePath == "" || url.AbsolutePath == "/")
            {
                result.AddResultRule("root_node", ResultType.Success);
            }
            else
            {
                var keywordUrl = pageData.Focuskeyword.UrlFriendly();
                if (url.AbsolutePath.Contains(keywordUrl))
                {
                    result.AddResultRule("url_contains_keyword", ResultType.Success);
                }
                else
                {
                    result.AddResultRule("url_doesnt_contain_keyword", ResultType.Hint);
                }
            }

            return result;
        }
    }
}
