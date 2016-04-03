using System.Diagnostics;
using System.Net;
using System.Text;
using System.Xml.Linq;
using HtmlParserSharp;
using RankOne.Business.Analyzers;
using RankOne.Business.Models;

namespace RankOne.Business.Services
{
    public class AnalyzeService
    {
        private SimpleHtmlParser _htmlParser;

        public AnalyzeService()
        {
            _htmlParser = new SimpleHtmlParser();
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

                var htmlAnalyzer = new HtmlAnalyzer(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Title = "htmlanalyzer_title",
                    Analysis = htmlAnalyzer.GetAnalysis()
                });

                var keywordAnalyzer = new KeywordAnalyzer(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Title = "keywordanalyzer_title",
                    Analysis = keywordAnalyzer.GetAnalysis()
                });

                var speedAnalyzer = new SpeedAnalyzer(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Title = "speedanalyzer_title",
                    Analysis = speedAnalyzer.GetAnalysis()
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
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var html = new WebClient().DownloadString(url);

            stopwatch.Stop();

            var xmlDocument = _htmlParser.ParseString(html);
            var xDocument = XDocument.Parse(xmlDocument.OuterXml);

            return new HtmlResult
            {
                Url = url,
                Html = html,
                Size = Encoding.ASCII.GetByteCount(html),
                ServerResponseTime = stopwatch.ElapsedMilliseconds,
                Document = xDocument
            };
        }
    }
}
