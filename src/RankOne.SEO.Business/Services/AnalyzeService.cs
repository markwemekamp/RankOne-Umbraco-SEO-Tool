using System.Diagnostics;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using RankOne.Business.Models;
using RankOne.Business.Summaries;

namespace RankOne.Business.Services
{
    public class AnalyzeService
    {
        private readonly HtmlDocument _htmlParser;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
        }

        public PageAnalysis AnalyzeWebPage(string url)
        {
            var webpage = new PageAnalysis
            {
                Url = url,
            };

            try
            {
                webpage.HtmlResult = GetHtml(url);

                var htmlAnalyzer = new HtmlSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "htmlanalyzer",
                    Analysis = htmlAnalyzer.GetAnalysis()
                });

                var keywordAnalyzer = new KeywordSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "keywordanalyzer",
                    Analysis = keywordAnalyzer.GetAnalysis()
                });

                var performanceAnalyzer = new PerformanceSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "performanceanalyzer",
                    Analysis = performanceAnalyzer.GetAnalysis()
                });
            }
            catch (WebException ex)
            {
                webpage.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }
            return webpage;
        }

        private HtmlResult GetHtml(string url)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var html = new WebClient().DownloadString(url);

            stopwatch.Stop();

            _htmlParser.LoadHtml(html);

            return new HtmlResult
            {
                Url = url,
                Html = html,
                Size = Encoding.ASCII.GetByteCount(html),
                ServerResponseTime = stopwatch.ElapsedMilliseconds,
                Document = _htmlParser.DocumentNode
        };
        }
    }
}
