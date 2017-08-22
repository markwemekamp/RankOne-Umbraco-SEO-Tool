using RankOne.Interfaces;
using System.Net;

namespace RankOne.Helpers
{
    public class WebRequestHelper : IWebRequestHelper
    {
        public HttpStatusCode GetStatus(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
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
            return GetStatus(url) == HttpStatusCode.OK;
        }
    }
}
