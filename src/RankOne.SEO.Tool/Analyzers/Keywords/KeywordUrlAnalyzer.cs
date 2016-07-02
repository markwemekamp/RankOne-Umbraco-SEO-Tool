using System;
using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    public class KeywordUrlAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordurlanalyzer"
            };

            var keyword = additionalValues[0].ToString();
            var urlString = additionalValues[1].ToString();
            var url = new Uri(urlString);

            if (url.AbsolutePath == "" || url.AbsolutePath == "/")
            {
                result.AddResultRule("keywordurlanalyzer_root_node", ResultType.Success);
            }
            else
            {
                var keywordUrl = keyword.Alias();
                if (url.AbsolutePath.Contains(keywordUrl))
                {
                    result.AddResultRule("keywordurlanalyzer_url_contains_keyword", ResultType.Success);
                }
                else
                {
                    result.AddResultRule("keywordurlanalyzer_url_doesnt_contain_keyword", ResultType.Hint);
                }
            }

            return result;
        }
    }
}
