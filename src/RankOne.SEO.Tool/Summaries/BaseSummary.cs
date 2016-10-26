using System;
using RankOne.Analyzers;
using RankOne.Models;
using RankOne.Services;

namespace RankOne.Summaries
{
    public class BaseSummary
    {
        public HtmlResult HtmlResult { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var reflectionService = new ReflectionService();
            var types = reflectionService.GetAllAnalyzersForSummary(Name);

            foreach (var type in types)
            {
                // Create an instance of the found type and call the Analyse method
                var instance = Activator.CreateInstance(type);
                var result = ((BaseAnalyzer) instance).Analyse(HtmlResult.Document, FocusKeyword, Url);
                analysis.Results.Add(result);
            }

            return analysis;
        }
    }
}
