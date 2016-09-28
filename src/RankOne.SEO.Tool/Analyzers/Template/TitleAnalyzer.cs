using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
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
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "titleanalyzer"
            };

            var headTag = HtmlHelper.GetElements(document, "head");
            if (headTag.Any())
            {
                var titleTags = HtmlHelper.GetElements(headTag.First(), "title");
                if (!titleTags.Any())
                {
                    result.AddResultRule("titleanalyzer_no_title_tag", ResultType.Error);
                }
                else if (titleTags.Count() > 1)
                {
                    result.AddResultRule("titleanalyzer_multiple_title_tags", ResultType.Error);
                }
                else
                {
                    var firstTitleTag = titleTags.FirstOrDefault();
                    if (firstTitleTag != null)
                    {
                        var titleValue = firstTitleTag.InnerText;

                        if (string.IsNullOrWhiteSpace(titleValue))
                        {
                            result.AddResultRule("titleanalyzer_no_title_value", ResultType.Error);
                        }
                        else
                        {
                            titleValue = titleValue.Trim();

                            if (titleValue.Length > 60)
                            {
                                result.AddResultRule("titleanalyzer_title_too_long", ResultType.Warning);
                            }

                            if (titleValue.Length < 5)
                            {
                                result.AddResultRule("titleanalyzer_title_too_short", ResultType.Hint);
                            }

                            if (titleValue.Length <= 60 && titleValue.Length >= 5)
                            {
                                result.AddResultRule("titleanalyzer_title_success", ResultType.Success);
                            }
                        }
                    }
                }
            }
            else
            {
                result.AddResultRule("titleanalyzer_no_head_tag", ResultType.Error);
            }
            return result;
        }
    }
}
