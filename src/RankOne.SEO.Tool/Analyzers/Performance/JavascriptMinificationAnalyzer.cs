using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class JavascriptMinificationAnalyzer : MinificationAnalyzer
    {
        public JavascriptMinificationAnalyzer() : this(RankOneContext.Instance)
        { }

        public JavascriptMinificationAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.MinificationHelper.Value, rankOneContext.CacheHelper.Value, 
            rankOneContext.UrlHelper.Value)
        { }

        public JavascriptMinificationAnalyzer(IMinificationHelper minificationHelper, ICacheHelper cacheHelper, IUrlHelper urlHelper) : 
            base(minificationHelper, cacheHelper, urlHelper)
        { }

        protected override string CacheKeyPrefix
        {
            get
            {
                return "js_minified_";
            }
        }

        protected override HtmlAttribute GetAttribute(HtmlNode node)
        {
            return node.GetAttribute("src");
        }

        protected override IEnumerable<HtmlNode> GetFiles(IPageData pageData, Uri url)
        {
            return pageData.Document.GetElementsWithAttribute("script", "src").
                Where(x =>
                    x.Attributes.Any(y => y.Name == "src" && y.Value.EndsWith("js") && ((y.Value.StartsWith("/") && !y.Value.StartsWith("//"))
                                                                                        || y.Value.StartsWith(url.Host)
                        ))
                );
        }
    }
}