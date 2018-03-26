using HtmlAgilityPack;
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
        private HashSet<string> _brokenLinks;

        public BrokenLinkAnalyzer() : this(RankOneContext.Instance)
        { }

        public BrokenLinkAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.UrlStatusService.Value, rankOneContext.UrlHelper.Value,
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
            _brokenLinks = new HashSet<string>();
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var anchorTags = pageData.Document.GetElements("a");
            var anchorTagCount = anchorTags.Count();

            var url = new Uri(pageData.Url);

            foreach (var anchorTag in anchorTags)
            {
                CheckAnchor(anchorTag, url);
            }

            if (!_brokenLinks.Any())
            {
                AddResultRule("all_links_working", ResultType.Success);
            }
            else
            {
                foreach (var brokenLink in _brokenLinks)
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

        private void CheckAnchor(HtmlNode anchorTag, Uri url)
        {
            if (anchorTag.GetAttribute("href") != null)
            {
                var hrefValue = anchorTag.GetAttribute("href").Value;

                if (hrefValue != null && !_brokenLinks.Contains(hrefValue) && !string.IsNullOrWhiteSpace(hrefValue) && hrefValue != "/" && hrefValue != "#")
                {
                    CheckUrl(hrefValue, url);
                }
            }
        }

        private void CheckUrl(string hrefValue, Uri url)
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
                _brokenLinks.Add(hrefValue);
            }
        }
    }
}