using System;
using System.Linq;
using System.Net;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    [AnalyzerCategory(SummaryName = "Performance", Alias = "javascriptminificationanalyzer")]
    public class JavascriptMinificationAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "javascriptminificationanalyzer"
            };

            var url = new Uri(pageData.Url);

            var localCssFiles = pageData.Document.GetDescendingElementsWithAttribute("script", "src").
                Where(x =>
                        x.Attributes.Any(y => y.Name == "src" && y.Value.EndsWith("js") && ((y.Value.StartsWith("/") && !y.Value.StartsWith("//"))
                            || y.Value.StartsWith(url.Host)
                        ))
                );

            var webClient = new WebClient();

            foreach (var localCssFile in localCssFiles)
            {
                var address = localCssFile.GetAttribute("src");

                if (address != null)
                {
                    var fullPath = address.Value;
                    if (fullPath.StartsWith("/"))
                    {
                        fullPath = string.Format("{0}://{1}{2}", url.Scheme, url.Host, fullPath);
                    }

                    try
                    {
                        var content = webClient.DownloadString(fullPath);

                        var totalCharacters = content.Length;
                        var lines = content.Count(x => x == '\n');

                        var ratio = totalCharacters / lines;

                        if (ratio < 200)
                        {
                            var resultRule = new ResultRule
                            {
                                Alias = "javascriptminificationanalyzer_file_not_minified",
                                Type = ResultType.Hint
                            };
                            resultRule.Tokens.Add(address.Value);
                            result.ResultRules.Add(resultRule);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            if (!result.ResultRules.Any())
            {
                result.AddResultRule("javascriptminificationanalyzer_all_minified", ResultType.Success);
            }

            return result;
        }
    }
}
