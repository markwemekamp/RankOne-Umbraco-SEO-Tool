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


            var gzipAnalysis = new AnalyzeResult
            {
                Alias = "gzipanalyzer"
            };
            var gzipResultRule = new ResultRule();
            if (encoding == "gzip")
            {
                gzipResultRule.Code = "gzipanalyzer_gzip_enabled";
                gzipResultRule.Type = ResultType.Success;
            }
            else
            {
                gzipResultRule.Code = "gzipanalyzer_gzip_disabled";
                gzipResultRule.Type = ResultType.Error;
            }
            gzipAnalysis.ResultRules.Add(gzipResultRule);

            return gzipAnalysis;
        }
    }
}
