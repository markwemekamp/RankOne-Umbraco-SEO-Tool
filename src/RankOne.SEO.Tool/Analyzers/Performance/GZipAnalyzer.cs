using System.Net;
using RankOne.Attributes;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    [AnalyzerCategory(SummaryName = "Performance", Alias = "gzipanalyzer")]
    public class GZipAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            string encoding = null;
            var request = (HttpWebRequest)WebRequest.Create(pageData.Url);
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    encoding = response.ContentEncoding;
                }
            }

            var result = new AnalyzeResult();
            if (encoding == "gzip")
            {
                result.AddResultRule("gzip_enabled", ResultType.Success);
            }
            else
            {
                result.AddResultRule("gzip_disabled", ResultType.Hint);
            }

            return result;
        }
    }
}
