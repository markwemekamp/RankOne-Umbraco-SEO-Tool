using RankOne.Attributes;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;

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
        private readonly HtmlTagHelper _htmlTagHelper;

        public KeywordTitleAnalyzer()
        {
            _htmlTagHelper = new HtmlTagHelper();
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var titleTag = _htmlTagHelper.GetTitleTag(pageData.Document, result);
            if (titleTag != null)
            {
                var titleText = titleTag.InnerText;
                var position = titleText.IndexOf(pageData.Focuskeyword, StringComparison.InvariantCultureIgnoreCase);

                if (position >= 0)
                {
                    if (position < 10)
                    {
                        result.AddResultRule("title_contains_keyword", ResultType.Success);
                    }
                    else
                    {
                        result.AddResultRule("title_not_in_front", ResultType.Hint);
                    }
                }
                else
                {
                    result.AddResultRule("title_doesnt_contain_keyword", ResultType.Warning);
                }
            }

            return result;
        }
    }
}