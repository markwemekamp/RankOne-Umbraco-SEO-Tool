using System;
using System.Net;
using System.Text;
using RankOne.Business.Analyzers;
using RankOne.Business.Analyzers.Speed;
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

            var serverResponseAnalyzer = new ServerResponseAnalyzer();
            analysis.Results.Add(serverResponseAnalyzer.Analyse(_htmlResult.Document, _htmlResult.ServerResponseTime));

            var gzipAnalyzer = new GZipAnalyzer();
            analysis.Results.Add(gzipAnalyzer.Analyse(_htmlResult.Document, _htmlResult.Url));

            var htmlSizeAnalyzer = new HtmlSizeAnalyzer();
            analysis.Results.Add(htmlSizeAnalyzer.Analyse(_htmlResult.Document));


            var externalCallAnalyzer = new AdditionalCallAnalyzer();
            analysis.Results.Add(externalCallAnalyzer.Analyse(_htmlResult.Document));

            //var cssMinifationAnalyzer = new CssMinificationAnalyzer();
            //analysis.Results.Add(cssMinifationAnalyzer.Analyse(_htmlResult.Document));

            return analysis;
        }
    }
}