using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Services
{
    public class UrlStatusService : IUrlStatusService
    {
        private IWebRequestHelper _webRequestHelper;
        private ICacheHelper _statusCache;

        public UrlStatusService() : this(RankOneContext.Instance)
        { }

        public UrlStatusService(IRankOneContext rankOneContext) : this(rankOneContext.WebRequestHelper.Value, rankOneContext.CacheHelper.Value)
        { }

        public UrlStatusService(IWebRequestHelper webRequestHelper, ICacheHelper cacheHelper)
        {
            if (webRequestHelper == null) throw new ArgumentNullException(nameof(webRequestHelper));
            if (cacheHelper == null) throw new ArgumentNullException(nameof(cacheHelper));

            _webRequestHelper = webRequestHelper;
            _statusCache = cacheHelper;
        }

        public bool IsActiveUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var cacheKey = $"url_status{url}";

            if (!_statusCache.Exists(cacheKey))
            {
                _statusCache.SetValue(cacheKey, _webRequestHelper.IsActiveUrl(url));
            }
            return _statusCache.GetValue(cacheKey).ToString() == true.ToString();
        }
    }
}