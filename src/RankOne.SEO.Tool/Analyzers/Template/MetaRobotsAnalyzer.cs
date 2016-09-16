using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    /// Analyzer for checking robots meta data tag related optimizations
    /// 
    /// Sources: https://support.google.com/webmasters/answer/79812?hl=en, SEO for 2016 by Sean Odom
    /// 
    /// </summary>
    [AnalyzerCategory(SummaryName = "Template")]
    public class MetaRobotsAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "metarobotsanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.AddResultRule("metarobotsanalyzer_no_meta_tags", ResultType.Error);
            }
            else
            {
                var robots = from metaTag in metaTags
                             let attribute = HtmlHelper.GetAttribute(metaTag, "name")
                             where attribute != null
                             where attribute.Value == "robots"
                             select HtmlHelper.GetAttribute(metaTag, "content");

                var googlebot = from metaTag in metaTags
                                let attribute = HtmlHelper.GetAttribute(metaTag, "name")
                                where attribute != null
                                where attribute.Value == "googlebot"
                                select HtmlHelper.GetAttribute(metaTag, "content");

                if (!robots.Any() && !googlebot.Any())
                {
                    result.AddResultRule("metarobotsanalyzer_no_robots_tag", ResultType.Success);
                }
                else
                {
                    var firstRobotTag = robots.FirstOrDefault();
                    if (firstRobotTag != null)
                    {
                        AnalyzeTag(firstRobotTag.Value, result, "robots");
                    }

                    var firstGooglebotTag = googlebot.FirstOrDefault();
                    if (firstGooglebotTag != null)
                    {
                        AnalyzeTag(firstGooglebotTag.Value, result, "googlebot");
                    }

                }
            }
            return result;
        }

        private static void AnalyzeTag(string tagValue, AnalyzeResult result, string tag)
        {
            if (tagValue.Contains("none"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_none", ResultType.Error);
            }
            if (tagValue.Contains("noindex"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_index", ResultType.Error);
            }
            if (tagValue.Contains("nofollow"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_follow", ResultType.Warning);
            }
            if (tagValue.Contains("nosnippet"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_snippet", ResultType.Information);
            }
            if (tagValue.Contains("noodp"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_odp", ResultType.Information);
            }
            if (tagValue.Contains("noarchive"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_archive", ResultType.Information);
            }
            if (tagValue.Contains("unavailable_after"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_unavailable_after", ResultType.Information);
            }
            if (tagValue.Contains("noimageindex"))
            {
                result.AddResultRule("metarobotsanalyzer_" + tag + "_no_image_index", ResultType.Information);
            }
        }
    }
}
