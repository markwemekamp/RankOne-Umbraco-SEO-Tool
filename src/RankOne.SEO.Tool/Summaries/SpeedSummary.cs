using System;
using RankOne.Analyzers.Speed;
using RankOne.Models;

namespace RankOne.Summaries
{
    public class PerformanceSummary
    {
        private readonly HtmlResult _htmlResult;

        public string Url { get; set; }

        public PerformanceSummary(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var gzipAnalyzer = new GZipAnalyzer();
            analysis.Results.Add(gzipAnalyzer.Analyse(_htmlResult.Document, Url));

            var htmlSizeAnalyzer = new HtmlSizeAnalyzer();
            analysis.Results.Add(htmlSizeAnalyzer.Analyse(_htmlResult.Document));

            var externalCallAnalyzer = new AdditionalCallAnalyzer();
            analysis.Results.Add(externalCallAnalyzer.Analyse(_htmlResult.Document));

            var url = new Uri(Url);

            var cssMinifationAnalyzer = new CssMinificationAnalyzer();
            analysis.Results.Add(cssMinifationAnalyzer.Analyse(_htmlResult.Document, url));

            var javascriptMinifationAnalyzer = new JavascriptMinificationAnalyzer();
            analysis.Results.Add(javascriptMinifationAnalyzer.Analyse(_htmlResult.Document, url));

            return analysis;
        }
    }
}