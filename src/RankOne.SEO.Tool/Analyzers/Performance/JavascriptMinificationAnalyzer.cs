using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RankOne.Analyzers.Performance
{
    public class JavascriptMinificationAnalyzer : BaseAnalyzer
    {
        private readonly IMinificationHelper _minificationHelper;

        public JavascriptMinificationAnalyzer() : this(RankOneContext.Instance)
        { }

        public JavascriptMinificationAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.MinificationHelper.Value)
        { }

        public JavascriptMinificationAnalyzer(IMinificationHelper minificationHelper)
        {
            _minificationHelper = minificationHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var url = new Uri(pageData.Url);

            var localJsFiles = GetLocalJsFiles(pageData, url);

            foreach (var localJsFile in localJsFiles)
            {
                CheckFile(localJsFile, url, result);
            }
            if (!result.ResultRules.Any())
            {
                result.AddResultRule("all_minified", ResultType.Success);
            }

            return result;
        }

        private void CheckFile(HtmlNode localJsFile, Uri url, AnalyzeResult result)
        {
            var address = localJsFile.GetAttribute("src");

            if (address != null)
            {
                var fullPath = address.Value;
                var content = GetContent(fullPath, url);
                if (content != null)
                {
                    var isMinified = _minificationHelper.IsMinified(content);

                    if (isMinified)
                    {
                        var resultRule = new ResultRule
                        {
                            Alias = "file_not_minified",
                            Type = ResultType.Hint
                        };
                        resultRule.Tokens.Add(fullPath);
                        result.ResultRules.Add(resultRule);
                    }
                }
            }
        }

        private string GetContent(string fullPath, Uri url)
        {
            if (fullPath.StartsWith("/"))
            {
                fullPath = string.Format("{0}://{1}{2}", url.Scheme, url.Host, fullPath);
            }

            try
            {
                var webClient = new WebClient();
                return webClient.DownloadString(fullPath);
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        private IEnumerable<HtmlNode> GetLocalJsFiles(IPageData pageData, Uri url)
        {
            return pageData.Document.GetElementsWithAttribute("script", "src").
                Where(x =>
                    x.Attributes.Any(y => y.Name == "src" && y.Value.EndsWith("js") && ((y.Value.StartsWith("/") && !y.Value.StartsWith("//"))
                                                                                        || y.Value.StartsWith(url.Host)
                        ))
                );
        }
    }
}