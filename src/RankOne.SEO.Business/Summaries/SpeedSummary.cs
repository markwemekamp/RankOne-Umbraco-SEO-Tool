using System;
using System.Net;
using System.Text;
using RankOne.Business.Analyzers;
using RankOne.Business.Models;

namespace RankOne.Business.Summaries
{
    public class SpeedSummary
    {
        private readonly HtmlResult _htmlResult;

        public SpeedSummary(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var serverResponseAnalysis = new AnalyzeResult();
            serverResponseAnalysis.Alias = "serverresponseanalyzer";
            serverResponseAnalysis.Title = "serverresponseanalyzer_title";
            var serverResponseAnalysisResultRule = new ResultRule { Code = "serverresponseanalyzer_responsetime", Type = _htmlResult.ServerResponseTime > 3 ? ResultType.Warning : ResultType.Success };
            serverResponseAnalysisResultRule.Tokens.Add(_htmlResult.ServerResponseTime.ToString());
            serverResponseAnalysis.ResultRules.Add(serverResponseAnalysisResultRule);
            analysis.Results.Add(serverResponseAnalysis);


            string encoding = null;
            var request = (HttpWebRequest)WebRequest.Create(_htmlResult.Url);
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    encoding = response.ContentEncoding;
                }
            }


            var gzipAnalysis = new AnalyzeResult();
            gzipAnalysis.Alias = "gzipanalyzer";
            gzipAnalysis.Title = "gzipanalyzer_title";
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
            analysis.Results.Add(gzipAnalysis);


            var htmlSizeAnalysis = new AnalyzeResult();
            htmlSizeAnalysis.Alias = "htmlsizeanalyzer";
            htmlSizeAnalysis.Title = "htmlsizeanalyzer_title";
            var byteCount = Encoding.Unicode.GetByteCount(_htmlResult.Html);
            var htmlSizeResultRule = new ResultRule();
            if (byteCount < (33 * 1024))
            {
                htmlSizeResultRule.Code = "htmlsizeanalyzer_html_size_small";
                htmlSizeResultRule.Type = ResultType.Success;
            }
            else
            {
                htmlSizeResultRule.Code = "htmlsizeanalyzer_html_size_too_large";
                htmlSizeResultRule.Type = ResultType.Warning;
            }
            htmlSizeResultRule.Tokens.Add(SizeSuffix(byteCount));
            htmlSizeAnalysis.ResultRules.Add(htmlSizeResultRule);

            analysis.Results.Add(htmlSizeAnalysis);


            var externalCallAnalyzer = new AdditionalCallAnalyzer();
            analysis.Results.Add(externalCallAnalyzer.Analyse(_htmlResult.Document));

            //var cssMinifationAnalyzer = new CssMinificationAnalyzer();
            //analysis.Results.Add(cssMinifationAnalyzer.Analyse(_htmlResult.Document));


            return analysis;
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(int value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }

}