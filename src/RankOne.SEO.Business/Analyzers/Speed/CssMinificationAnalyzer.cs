using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Speed
{
    public class CssMinificationAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "cssminificationanalyzer"
            };

            var url = (Uri)additionalValues[0];

            var localCssFiles = HtmlHelper.GetElementsWithAttribute(document, "link", "href").
                Where(x =>
                        x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet") &&
                        x.Attributes.Any(y => y.Name == "href" && ((y.Value.StartsWith("/") && !y.Value.StartsWith("//"))
                            || y.Value.StartsWith(url.Host)
                        ))
                );

            var webClient = new WebClient();

            foreach (var localCssFile in localCssFiles)
            {
                var address = HtmlHelper.GetAttribute(localCssFile, "href");

                if (address != null)
                {
                    var fullPath = address.Value;
                    if (fullPath.StartsWith("/"))
                    {
                        fullPath = string.Format("{0}://{1}{2}", url.Scheme, url.Host, fullPath);
                    }

                    var content = webClient.DownloadString(fullPath);

                    var totalCharacters = content.Length;
                    var lines = content.Count(x => x == '\n');

                    var ratio = totalCharacters / lines;

                    if (ratio < 200)
                    {
                        var resultRule = new ResultRule();
                        resultRule.Code = "cssminificationanalyzer_file_not_minified";
                        resultRule.Type = ResultType.Hint;
                        resultRule.Tokens.Add(address.Value);
                        result.ResultRules.Add(resultRule);
                    }

                }
            }
            if (!result.ResultRules.Any())
            {
                result.AddResultRule("cssminificationanalyzer_all_minified", ResultType.Success);
            }

            return result;
        }
    }
}
