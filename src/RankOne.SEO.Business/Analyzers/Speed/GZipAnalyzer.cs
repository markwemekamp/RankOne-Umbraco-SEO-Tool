using System.Net;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Speed
{
    public class GZipAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var url = additionalValues[0].ToString();
            string encoding = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    encoding = response.ContentEncoding;
                }
            }

            var result = new AnalyzeResult
            {
                Alias = "gzipanalyzer"
            };
            if (encoding == "gzip")
            {
                result.AddResultRule("gzipanalyzer_gzip_enabled", ResultType.Success);
            }
            else
            {
                result.AddResultRule("gzipanalyzer_gzip_disabled", ResultType.Hint);
            }

            return result;
        }
    }
}
