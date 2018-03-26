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

        public GZipAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.EncodingHelper.Value, rankOneContext.CacheHelper.Value)
        { }

        public GZipAnalyzer(IEncodingHelper encodingHelper, ICacheHelper cacheHelper) : base()
        {
            if (encodingHelper == null) throw new ArgumentNullException(nameof(encodingHelper));
            if (cacheHelper == null) throw new ArgumentNullException(nameof(cacheHelper));

            _encodingHelper = encodingHelper;
            _cacheHelper = cacheHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

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
                AddResultRule("gzip_enabled", ResultType.Success);
            }
            else
            {
                AddResultRule("gzip_disabled", ResultType.Hint);
            }
        }
    }
}