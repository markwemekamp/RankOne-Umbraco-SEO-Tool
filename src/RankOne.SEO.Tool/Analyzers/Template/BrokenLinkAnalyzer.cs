using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class BrokenLinkAnalyzer : BaseAnalyzer
    {
        private IUrlStatusService _urlStatusService;

        public BrokenLinkAnalyzer() : this(RankOneContext.Instance)
        { }

        public BrokenLinkAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.UrlStatusService.Value)
        { }

        public BrokenLinkAnalyzer(IUrlStatusService urlStatusService)
        {
            _urlStatusService = urlStatusService;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var anchorTags = pageData.Document.GetElements("a");
            var anchorTagCount = anchorTags.Count();

            var url = new Uri(pageData.Url);

            var brokenLinks = new HashSet<string>();

            foreach (var anchorTag in anchorTags)
            {
                if (anchorTag.GetAttribute("href") != null)
                {
                    var hrefValue = anchorTag.GetAttribute("href").Value;

                    if (hrefValue != null && !brokenLinks.Contains(hrefValue) && !string.IsNullOrWhiteSpace(hrefValue) && hrefValue != "/" && hrefValue != "#")
                    {
                        var fullUrl = hrefValue;
                        if (IsLocalLink(hrefValue))
                        {
                            var portSegment = "";
                            if (url.Port > 0)
                            {
                                portSegment = string.Format(":{0}", url.Port);
                            }

                            fullUrl = string.Format("{0}://{1}{3}{2}", url.Scheme, url.Host, hrefValue, portSegment);
                        }

                        var active = _urlStatusService.IsActiveUrl(fullUrl);

                        if (!active)
                        {
                            brokenLinks.Add(hrefValue);
                        }
                    }
                }
            }

            if (!brokenLinks.Any())
            {
                result.AddResultRule("all_links_working", ResultType.Success);
            }
            else
            {
                result.ResultRules.AddRange(brokenLinks.Select(x => new ResultRule() { Alias = "broken_link", Type = ResultType.Warning, Tokens = new List<string>() { x } }));
            }

            return result;
        }

        private bool IsLocalLink(string value)
        {
            return value.StartsWith("/");
        }
    }
}