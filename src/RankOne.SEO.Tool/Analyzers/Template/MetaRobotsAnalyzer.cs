using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Exceptions;
using System;
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

        public MetaRobotsAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public MetaRobotsAnalyzer(IHtmlTagHelper htmlTagHelper) : base()
        {
            if (htmlTagHelper == null) throw new ArgumentNullException(nameof(htmlTagHelper));

            _htmlTagHelper = htmlTagHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            try
            {
                var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document);

                if (metaTags.Any())
                {
                    AnalyzeMetaTags(metaTags);
                }
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Error);

            }
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags)
        {
            var robotTags = GetContentAttributeFromMetaTag(metaTags, "robots");
            var googlebotTags = GetContentAttributeFromMetaTag(metaTags, "googlebot");

            if (!robotTags.Any() && !googlebotTags.Any())
            {
                AddResultRule("no_robots_tag", ResultType.Success);
            }
            if (robotTags.Count() > 1)
            {
                AddResultRule("multiple_robots_tags", ResultType.Error);
            }
            if (googlebotTags.Count() > 1)
            {
                AddResultRule("multiple_googlebot_tags", ResultType.Error);
            }
            else
            {
                var firstRobotTag = robotTags.FirstOrDefault();
                if (firstRobotTag != null)
                {
                    AnalyzeRobotTag(firstRobotTag);
                }

                var firstGooglebotTag = googlebotTags.FirstOrDefault();
                if (firstGooglebotTag != null)
                {
                    AnalyzeGoogleBotTag(firstGooglebotTag);
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

        private void AnalyzeRobotTag(HtmlAttribute robotTag)
        {
            AnalyzeTag(robotTag.Value, "robots");
        }

        private void AnalyzeGoogleBotTag(HtmlAttribute googleBotTag)
        {
            AnalyzeTag(googleBotTag.Value, "googlebot");
        }

        private void AnalyzeTag(string tagValue, string tag)
        {
            if (tagValue.Contains("none"))
            {
                AddResultRule(tag + "_none", ResultType.Error);
            }
            if (tagValue.Contains("noindex"))
            {
                AddResultRule(tag + "_no_index", ResultType.Error);
            }
            if (tagValue.Contains("nofollow"))
            {
                AddResultRule(tag + "_no_follow", ResultType.Warning);
            }
            if (tagValue.Contains("nosnippet"))
            {
                AddResultRule(tag + "_no_snippet", ResultType.Information);
            }
            if (tagValue.Contains("noodp"))
            {
                AddResultRule(tag + "_no_odp", ResultType.Information);
            }
            if (tagValue.Contains("noarchive"))
            {
                AddResultRule(tag + "_no_archive", ResultType.Information);
            }
            if (tagValue.Contains("unavailable_after"))
            {
                AddResultRule(tag + "_unavailable_after", ResultType.Information);
            }
            if (tagValue.Contains("noimageindex"))
            {
                AddResultRule(tag + "_no_image_index", ResultType.Information);
            }
        }
    }
}