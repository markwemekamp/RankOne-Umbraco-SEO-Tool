using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Analyzers.Performance
{
    public class GZipAnalyzer : BaseAnalyzer
    {
        private readonly IEncodingHelper _encodingHelper;
        private readonly ICacheHelper _cacheHelper;

        public GZipAnalyzer() : this(RankOneContext.Instance)
        { }

        public GZipAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.EncodingHelper.Value, rankOneContext.CacheHelper.Value)
        { }

        public GZipAnalyzer(IEncodingHelper encodingHelper, ICacheHelper cacheHelper)
        {
            _encodingHelper = encodingHelper;
            _cacheHelper = cacheHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };
            var uri = new Uri(pageData.Url);

            var cacheKey = $"encoding_{uri.Authority}";

            if (!_cacheHelper.Exists(cacheKey))
            {
                var encodingByUrl = _encodingHelper.GetEncodingByUrl(pageData.Url);

                _cacheHelper.SetValue(cacheKey, encodingByUrl);
            }

            var encoding = _cacheHelper.GetValue(cacheKey).ToString();

            if (encoding == "gzip")
            {
                result.AddResultRule("gzip_enabled", ResultType.Success);
            }
            else
            {
                result.AddResultRule("gzip_disabled", ResultType.Hint);
            }

            return result;
        }
    }
}