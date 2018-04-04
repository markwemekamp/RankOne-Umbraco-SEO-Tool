using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Net;

namespace RankOne.Helpers
{
    public class EncodingHelper : IEncodingHelper
    {
        private IHttpWebRequestFactory _webRequestFactory;

        public EncodingHelper() : this(RankOneContext.Instance)
        { }

        public EncodingHelper(IRankOneContext rankOneContext) : this(rankOneContext.WebRequestFactory.Value)
        { }

        public EncodingHelper(IHttpWebRequestFactory webRequestFactory)
        {
            if (webRequestFactory == null) throw new ArgumentNullException(nameof(webRequestFactory));

            _webRequestFactory = webRequestFactory;
        }

        public string GetEncodingByUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var request = _webRequestFactory.Create(url);
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    return response.ContentEncoding;
                }
            }

            return null;
        }
    }
}