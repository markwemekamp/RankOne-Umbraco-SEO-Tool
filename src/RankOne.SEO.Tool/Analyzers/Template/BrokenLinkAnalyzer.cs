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
        private IUrlHelper _urlHelper;
        private ICacheHelper _cacheHelper;

        public BrokenLinkAnalyzer() : this(RankOneContext.Instance)
        { }

        public BrokenLinkAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.UrlStatusService.Value, rankOneContext.UrlHelper.Value,
            rankOneContext.CacheHelper.Value)
        { }

        public BrokenLinkAnalyzer(IUrlStatusService urlStatusService, IUrlHelper urlHelper, ICacheHelper cacheHelper) : base()
        {
            if (urlStatusService == null) throw new ArgumentNullException(nameof(urlStatusService));
            if (urlHelper == null) throw new ArgumentNullException(nameof(urlHelper));
            if (cacheHelper == null) throw new ArgumentNullException(nameof(cacheHelper));

            _urlStatusService = urlStatusService;
            _urlHelper = urlHelper;
            _cacheHelper = cacheHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

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
                        var fullUrl = _urlHelper.GetFullPath(hrefValue, url);

                        var cacheKey = "brokenlink_{fullUrl}";

                        if (!_cacheHelper.Exists(cacheKey))
                        {
                            var active = _urlStatusService.IsActiveUrl(fullUrl);
                            _cacheHelper.SetValue(cacheKey, active.ToString());
                        }

                        if (_cacheHelper.GetValue(cacheKey).ToString() != true.ToString())
                        {
                            brokenLinks.Add(hrefValue);
                        }
                    }
                }
            }

            if (!brokenLinks.Any())
            {
                AddResultRule("all_links_working", ResultType.Success);
            }
            else
            {
                foreach (var brokenLink in brokenLinks)
                {
                    var resultRule = new ResultRule()
                    {
                        Alias = "broken_link",
                        Type = ResultType.Warning,
                        Tokens = new List<string>() { brokenLink, brokenLink }
                    };
                    AddResultRule(resultRule);
                }
            }
        }
    }
}