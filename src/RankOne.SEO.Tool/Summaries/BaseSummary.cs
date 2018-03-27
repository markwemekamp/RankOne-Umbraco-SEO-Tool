using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using Umbraco.Core.Logging;

namespace RankOne.Summaries
{
    public class BaseSummary : ISummary
    {
        public HtmlNode Document { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }
        public IEnumerable<IAnalyzer> Analyzers { get; set; }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var pageData = GetPageData();
            foreach (var analyzer in Analyzers)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                analyzer.Analyse(pageData);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                LogHelper.Debug<BaseSummary>($"Finished analysis for {analyzer.Alias}, time: {elapsedMs} ms");
                analysis.Results.Add(analyzer.AnalyzeResult);
            }

            return analysis;
        }

        private PageData GetPageData()
        {
            return new PageData
            {
                Document = Document,
                Focuskeyword = FocusKeyword,
                Url = Url
            };
        }
    }
}