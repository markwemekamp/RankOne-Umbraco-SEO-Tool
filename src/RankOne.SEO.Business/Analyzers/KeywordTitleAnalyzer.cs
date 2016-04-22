using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordtitleanalyzer"
            };

            var titleTags = HtmlHelper.GetElements(document, "title");
            var keyword = additionalValues[0].ToString();

            if (!titleTags.Any())
            {
                result.ResultRules.Add(new ResultRule
                {
                    Code = "keywordtitleanalyzer_no_title_tag",
                    Type = ResultType.Warning
                });
            }
            else if (titleTags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule
                {
                    Code = "keywordtitleanalyzer_multiple_title_tags",
                    Type = ResultType.Warning
                });
            }
            else
            {
                if (titleTags.First().InnerText.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = "keywordtitleanalyzer_title_contains_keyword",
                        Type = ResultType.Success
                    });
                }
                else
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = "keywordtitleanalyzer_title_doesnt_contain_keyword",
                        Type = ResultType.Hint
                    });
                }
            }

            return result;
        }
    }
}
