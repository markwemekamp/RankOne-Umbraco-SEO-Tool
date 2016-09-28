using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
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
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordtitleanalyzer")]
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordtitleanalyzer"
            };

            var titleTags = HtmlHelper.GetElements(document, "title");

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
                var position = titleText.IndexOf(focuskeyword, StringComparison.InvariantCultureIgnoreCase);

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
