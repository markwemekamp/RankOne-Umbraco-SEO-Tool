using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RankOne.Analyzers.Performance
{
    public abstract class MinificationAnalyzer : BaseAnalyzer
    {
        private readonly IMinificationHelper _minificationHelper;
        private readonly ICacheHelper _cacheHelper;
        private readonly IUrlHelper _urlHelper;

        public MinificationAnalyzer() : this(RankOneContext.Instance)
        { }

        public MinificationAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.MinificationHelper.Value, rankOneContext.CacheHelper.Value, rankOneContext.UrlHelper.Value)
        { }

        public MinificationAnalyzer(IMinificationHelper minificationHelper, ICacheHelper cacheHelper, IUrlHelper urlHelper)
        {
            _minificationHelper = minificationHelper;
            _cacheHelper = cacheHelper;
            _urlHelper = urlHelper;
        }

        protected abstract string CacheKeyPrefix { get; }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var url = new Uri(pageData.Url);

            var files = GetFiles(pageData, url);

            foreach (var file in files)
            {
                CheckFile(file, url, result);
            }

            if (!result.ResultRules.Any())
            {
                result.AddResultRule("all_minified", ResultType.Success);
            }

            return result;
        }

        private void CheckFile(HtmlNode file, Uri url, AnalyzeResult result)
        {
            var address = GetAttribute(file);

            if (address != null)
            {
                var fullPath = _urlHelper.GetFullPath(address.Value, url);
                var cacheKey = $"{CacheKeyPrefix}{fullPath}";

                if (!_cacheHelper.Exists(cacheKey))
                {
                    var isMinified = false;
                    var content = GetContent(fullPath);
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
                    result.ResultRules.Add(resultRule);
                }

            }
        }

        private string GetContent(string fullPath)
        {
            try
            {
                var webClient = new WebClient();
                return webClient.DownloadString(fullPath);
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        protected abstract HtmlAttribute GetAttribute(HtmlNode node);
        protected abstract IEnumerable<HtmlNode> GetFiles(IPageData pageData, Uri url);
    }
}