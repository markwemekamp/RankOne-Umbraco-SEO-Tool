using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    /// Analyzer for checking robots meta data tag related optimizations
    ///
    /// Sources: https://support.google.com/webmasters/answer/79812?hl=en, SEO for 2016 by Sean Odom
    ///
    /// </summary>
    public class MetaRobotsAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;

        public MetaRobotsAnalyzer() : this(RankOneContext.Instance)
        { }

        public MetaRobotsAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public MetaRobotsAnalyzer(IHtmlTagHelper htmlTagHelper)
        {
            _htmlTagHelper = htmlTagHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document, result);

            if (metaTags.Any())
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
                result.AddResultRule("no_robots_tag", ResultType.Success);
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
                result.AddResultRule(tag + "_none", ResultType.Error);
            }
            if (tagValue.Contains("noindex"))
            {
                result.AddResultRule(tag + "_no_index", ResultType.Error);
            }
            if (tagValue.Contains("nofollow"))
            {
                result.AddResultRule(tag + "_no_follow", ResultType.Warning);
            }
            if (tagValue.Contains("nosnippet"))
            {
                result.AddResultRule(tag + "_no_snippet", ResultType.Information);
            }
            if (tagValue.Contains("noodp"))
            {
                result.AddResultRule(tag + "_no_odp", ResultType.Information);
            }
            if (tagValue.Contains("noarchive"))
            {
                result.AddResultRule(tag + "_no_archive", ResultType.Information);
            }
            if (tagValue.Contains("unavailable_after"))
            {
                result.AddResultRule(tag + "_unavailable_after", ResultType.Information);
            }
            if (tagValue.Contains("noimageindex"))
            {
                result.AddResultRule(tag + "_no_image_index", ResultType.Information);
            }
        }
    }
}