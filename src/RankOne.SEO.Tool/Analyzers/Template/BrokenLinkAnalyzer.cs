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
        private IWebRequestHelper _webRequestHelper;

        public BrokenLinkAnalyzer() : this(RankOneContext.Instance)
        { }

        public BrokenLinkAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.WebRequestHelper.Value)
        { }

        public BrokenLinkAnalyzer(IWebRequestHelper webRequestHelper)
        {
            _webRequestHelper = webRequestHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var anchorTags = pageData.Document.GetElements("a");
            var anchorTagCount = anchorTags.Count();

            var url = new Uri(pageData.Url);

            foreach (var anchorTag in anchorTags)
            {
                if (anchorTag.GetAttribute("href") != null) {

                    var hrefValue = anchorTag.GetAttribute("href").Value;

                    if (hrefValue != null && !string.IsNullOrWhiteSpace(hrefValue) && hrefValue != "/" && hrefValue != "#")
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

                        var active = _webRequestHelper.IsActiveUrl(fullUrl);

                        if (!active)
                        {
                            result.ResultRules.Add(new ResultRule() { Alias = "broken_link", Type = ResultType.Warning, Tokens = new List<string>() { hrefValue } });
                        }

                    }
                }
            }

            if (!result.ResultRules.Any())
            {
                result.AddResultRule("all_links_working", ResultType.Success);
            }

            return result;
        }

        private bool IsLocalLink(string value)
        {
            return value.StartsWith("/");
        }
    }
}