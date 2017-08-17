using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Summaries
{
    public class BaseSummary : ISummary
    {
        public HtmlResult HtmlResult { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }
        public IEnumerable<IAnalyzer> Analyzers { get; set; }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();
            foreach (var analyzer in Analyzers)
            {
                var result = GetResultFromAnalyzer(analyzer);

                result.Alias = analyzer.Alias;
                analysis.Results.Add(result);
            }

            return analysis;
        }

        private AnalyzeResult GetResultFromAnalyzer(IAnalyzer analyzer)
        {
            var pageData = new PageData
            {
                Document = HtmlResult.Document,
                Focuskeyword = FocusKeyword,
                Url = Url
            };
            return analyzer.Analyse(pageData);
        }
    }
}