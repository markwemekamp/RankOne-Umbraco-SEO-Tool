using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public abstract class MinificationAnalyzer : BaseAnalyzer
    {
        private readonly IMinificationHelper _minificationHelper;
        private readonly ICacheHelper _cacheHelper;
        private readonly IUrlHelper _urlHelper;

        public MinificationAnalyzer(IMinificationHelper minificationHelper, ICacheHelper cacheHelper, IUrlHelper urlHelper) : base()
        {
            if (minificationHelper == null) throw new ArgumentNullException(nameof(minificationHelper));
            if (cacheHelper == null) throw new ArgumentNullException(nameof(cacheHelper));
            if (urlHelper == null) throw new ArgumentNullException(nameof(urlHelper));

            _minificationHelper = minificationHelper;
            _cacheHelper = cacheHelper;
            _urlHelper = urlHelper;
        }

        protected abstract string CacheKeyPrefix { get; }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var url = new Uri(pageData.Url);

            var files = GetFiles(pageData, url);

            foreach (var file in files)
            {
                CheckFile(file, url);
            }

            if (!AnalyzeResult.ResultRules.Any())
            {
                AnalyzeResult.AddResultRule("all_minified", ResultType.Success);
            }
        }

        private void CheckFile(HtmlNode file, Uri url)
        {
            var address = GetAttribute(file);

            if (address != null)
            {
                var fullPath = _urlHelper.GetFullPath(address.Value, url);
                var cacheKey = $"{CacheKeyPrefix}{fullPath}";

                if (!_cacheHelper.Exists(cacheKey))
                {
                    var isMinified = false;
                    var content = _urlHelper.GetContent(fullPath);
                    if (content != null)
                    {
                        isMinified = _minificationHelper.IsMinified(content);
                    }

                    _cacheHelper.SetValue(cacheKey, isMinified.ToString());
                }

                if (_cacheHelper.GetValue(cacheKey).ToString() != true.ToString())
                {
                    var resultRule = new ResultRule
                    {
                        Alias = "file_not_minified",
                        Type = ResultType.Hint
                    };
                    resultRule.Tokens.Add(fullPath);
                    AddResultRule(resultRule);
                }
            }
        }

        protected abstract HtmlAttribute GetAttribute(HtmlNode node);

        protected abstract IEnumerable<HtmlNode> GetFiles(IPageData pageData, Uri url);
    }
}