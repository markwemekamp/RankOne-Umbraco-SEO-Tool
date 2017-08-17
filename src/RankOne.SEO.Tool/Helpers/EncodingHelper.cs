using RankOne.Interfaces;
using System.Net;

namespace RankOne.Helpers
{
    public class EncodingHelper : IEncodingHelper
    {
        public string GetEncodingFromUrl(string url)
        {
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