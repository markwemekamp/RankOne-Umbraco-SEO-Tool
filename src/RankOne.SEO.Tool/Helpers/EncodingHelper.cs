using RankOne.Interfaces;
using System;
using System.Net;

namespace RankOne.Helpers
{
    public class EncodingHelper : IEncodingHelper
    {
        public string GetEncodingByUrl(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var request = (HttpWebRequest)WebRequest.Create(url);
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