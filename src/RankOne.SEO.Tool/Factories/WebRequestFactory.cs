using RankOne.Interfaces;
using System;
using System.Net;

namespace RankOne.Factories
{
    public class WebRequestFactory : IHttpWebRequestFactory
    {
        public HttpWebRequest Create(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            return (HttpWebRequest)WebRequest.Create(url);
        }
    }
}