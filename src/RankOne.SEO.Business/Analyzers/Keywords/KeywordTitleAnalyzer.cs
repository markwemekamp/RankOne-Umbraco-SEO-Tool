using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Keywords
{
    /// <summary>
    /// Analyzer for checking keyword in title tag related optimizations
    /// 
    /// https://moz.com/learn/seo/title-tag
    /// 
    /// 1. check for title tag - critical
    /// 2. check for multiple title tags - critical
    /// 3. check if title contains keyword - major
    /// 4. location of the keyword - minor
    /// </summary>
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
                result.AddResultRule("keywordtitleanalyzer_no_title_tag", ResultType.Error);

            }
            else if (titleTags.Count() > 1)
            {
                result.AddResultRule("keywordtitleanalyzer_multiple_title_tags", ResultType.Error);
            }
            else
            {
                var titleText = titleTags.First().InnerText;
                var position = titleText.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase);

                if (position >= 0)
                {
                    if (position < 10)
                    {
                        result.AddResultRule("keywordtitleanalyzer_title_contains_keyword", ResultType.Success);
                    }
                    else
                    {
                        result.AddResultRule("keywordtitleanalyzer_title_not_in_front", ResultType.Hint);
                    }
                }
                else
                {
                    result.AddResultRule("keywordtitleanalyzer_title_doesnt_contain_keyword", ResultType.Warning);
                }
            }

            return result;
        }
    }
}
