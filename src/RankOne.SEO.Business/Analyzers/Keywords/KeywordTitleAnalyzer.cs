using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Keywords
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
                result.AddResultRule("keywordtitleanalyzer_no_title_tag", ResultType.Warning);

            }
            else if (titleTags.Count() > 1)
            {
                result.AddResultRule("keywordtitleanalyzer_multiple_title_tags", ResultType.Warning);
            }
            else
            {
                if (titleTags.First().InnerText.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.AddResultRule("keywordtitleanalyzer_title_contains_keyword", ResultType.Success);
                }
                else
                {
                    result.AddResultRule("keywordtitleanalyzer_title_doesnt_contain_keyword", ResultType.Hint);
                }
            }

            return result;
        }
    }
}
