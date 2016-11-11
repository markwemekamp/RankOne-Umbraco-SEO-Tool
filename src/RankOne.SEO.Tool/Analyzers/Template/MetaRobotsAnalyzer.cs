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
    /// Analyzer for checking robots meta data tag related optimizations
    /// 
    /// Sources: https://support.google.com/webmasters/answer/79812?hl=en, SEO for 2016 by Sean Odom
    /// 
    /// </summary>
    [AnalyzerCategory(SummaryName = "Template", Alias = "metarobotsanalyzer")]
    public class MetaRobotsAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "metarobotsanalyzer"
            };

            var metaTags = pageData.Document.GetDescendingElements("meta");

            if (!metaTags.Any())
            {
                result.AddResultRule("metarobotsanalyzer_no_meta_tags", ResultType.Error);
            }
            else
            {
                AnalyzeMetaTags(metaTags, result);
            }
            return result;
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags, AnalyzeResult result)
        {
            var robots = GetContentAttributeFromMetaTag(metaTags, "robots");
            var googlebot = GetContentAttributeFromMetaTag(metaTags, "googlebots");

            if (!robots.Any() && !googlebot.Any())
            {
                result.AddResultRule("metarobotsanalyzer_no_robots_tag", ResultType.Success);
            }
            else
            {
                var firstRobotTag = robots.FirstOrDefault();
                if (firstRobotTag != null)
                {
                    AnalyzeRobotTag(firstRobotTag, result);
                }

                var firstGooglebotTag = googlebot.FirstOrDefault();
                if (firstGooglebotTag != null)
                {
                    AnalyzeGoogleBotTag(firstGooglebotTag, result);
                }

            }
        }

        private IEnumerable<HtmlAttribute> GetContentAttributeFromMetaTag(IEnumerable<HtmlNode> metaTags, string name)
        {
            return from metaTag in metaTags
                   let attribute = metaTag.GetAttribute("name")
                   where attribute != null
                   where attribute.Value == name
                   select metaTag.GetAttribute("content");
        }

        private void AnalyzeRobotTag(HtmlAttribute robotTag, AnalyzeResult result)
        {
            AnalyzeTag(robotTag.Value, result, "robots");
        }

        private void AnalyzeGoogleBotTag(HtmlAttribute googleBotTag, AnalyzeResult result)
        {
            AnalyzeTag(googleBotTag.Value, result, "googlebot");
        }

        private void AnalyzeTag(string tagValue, AnalyzeResult result, string tag)
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
