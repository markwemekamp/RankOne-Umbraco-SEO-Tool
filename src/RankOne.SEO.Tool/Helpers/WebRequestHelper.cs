using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Net;

namespace RankOne.Helpers
{
    public class WebRequestHelper : IWebRequestHelper
    {
        private IHttpWebRequestFactory _webRequestFactory;

        public WebRequestHelper() : this(RankOneContext.Instance)
        { }

        public WebRequestHelper(IRankOneContext rankOneContext) : this(rankOneContext.WebRequestFactory.Value)
        { }

        public WebRequestHelper(IHttpWebRequestFactory webRequestFactory)
        {
            if (webRequestFactory == null) throw new ArgumentNullException(nameof(webRequestFactory));

            _webRequestFactory = webRequestFactory;
        }

        private HttpStatusCode GetStatus(string url)
        {
            var request = _webRequestFactory.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch (WebException)
            {
                return HttpStatusCode.NotFound;
            }
        }

        public bool IsActiveUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            return GetStatus(url) == HttpStatusCode.OK;
        }
    }
}