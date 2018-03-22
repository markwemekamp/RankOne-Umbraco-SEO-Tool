using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class CssMinificationAnalyzer : MinificationAnalyzer
    {
        public CssMinificationAnalyzer() : this(RankOneContext.Instance)
        { }

        public CssMinificationAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.MinificationHelper.Value, rankOneContext.CacheHelper.Value, 
            rankOneContext.UrlHelper.Value)
        { }

        public CssMinificationAnalyzer(IMinificationHelper minificationHelper, ICacheHelper cacheHelper, IUrlHelper urlHelper) : 
            base(minificationHelper, cacheHelper, urlHelper)
        { }

        protected override string CacheKeyPrefix
        {
            get
            {
                return "css_minified_";
            }
        }

        protected override HtmlAttribute GetAttribute(HtmlNode node)
        {
            return node.GetAttribute("href");
        }

        protected override IEnumerable<HtmlNode> GetFiles(IPageData pageData, Uri url)
        {
            return pageData.Document.GetElementsWithAttribute("link", "href").
                Where(x =>
                        x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet") &&
                        x.Attributes.Any(y => y.Name == "href" && (((y.Value.StartsWith("/") || y.Value.StartsWith("../")) && !y.Value.StartsWith("//"))
                            || y.Value.StartsWith(url.Host)
                        ))
                );
        }
    }
}