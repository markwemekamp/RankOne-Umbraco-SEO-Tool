using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Services
{
    public class UrlStatusService : IUrlStatusService
    {
        private IWebRequestHelper _webRequestHelper;
        private Dictionary<string, bool> statusses;

        public UrlStatusService() : this(RankOneContext.Instance)
        { }

        public UrlStatusService(RankOneContext rankOneContext) : this(rankOneContext.WebRequestHelper.Value)
        { }

        public UrlStatusService(IWebRequestHelper webRequestHelper)
        {
            _webRequestHelper = webRequestHelper;
            statusses = new Dictionary<string, bool>();
        }

        public bool IsActiveUrl(string url)
        {
            if (!statusses.ContainsKey(url))
            {
                statusses.Add(url, _webRequestHelper.IsActiveUrl(url));
            }
            return statusses[url];
        }
    }
}