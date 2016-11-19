using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    /// Analyzer for checking title tag related optimizations
    /// 
    /// Sources: https://moz.com/learn/seo/title-tag, SEO for 2016 by Sean Odom
    /// 
    /// 1. check for head tag - critical
    /// 2. check for presence of title tag - critical
    /// 3. check for multiple title tags - critical
    /// 4. check for value of title tag - critical
    /// 5. check title tag length
    ///     1. longer than 60 - major
    ///     2. shorter than 10 - major
    /// </summary>
    [AnalyzerCategory(SummaryName = "Template", Alias = "titleanalyzer")]
    public class TitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var headTag = pageData.GetElements("head");
            if (headTag.Any())
            {
                AnalyzeHeadTag(headTag, result);
            }
            else
            {
                result.AddResultRule("no_head_tag", ResultType.Error);
            }
            return result;
        }

        private void AnalyzeHeadTag(IEnumerable<HtmlNode> headTag, AnalyzeResult result)
        {
            var titleTags = headTag.First().GetDescendingElements("title");
            if (!titleTags.Any())
            {
                result.AddResultRule("no_title_tag", ResultType.Error);
            }
            else if (titleTags.Count() > 1)
            {
                result.AddResultRule("multiple_title_tags", ResultType.Error);
            }
            else
            {
                var firstTitleTag = titleTags.FirstOrDefault();
                if (firstTitleTag != null)
                {
                    AnalyzeTitleTag(firstTitleTag, result);
                }
            }
        }

        private void AnalyzeTitleTag(HtmlNode titleTag, AnalyzeResult result)
        {
            var titleValue = titleTag.InnerText;

            if (string.IsNullOrWhiteSpace(titleValue))
            {
                result.AddResultRule("no_title_value", ResultType.Error);
            }
            else
            {
                titleValue = titleValue.Trim();

                if (titleValue.Length > 60)
                {
                    result.AddResultRule("title_too_long", ResultType.Warning);
                }

                if (titleValue.Length < 5)
                {
                    result.AddResultRule("titleanalyzer_title_too_short", ResultType.Hint);
                }

                if (titleValue.Length <= 60 && titleValue.Length >= 5)
                {
                    result.AddResultRule("title_success", ResultType.Success);
                }
            }
        }
    }
}
