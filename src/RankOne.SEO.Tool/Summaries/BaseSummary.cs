using System;
using System.Linq;
using System.Reflection;
using RankOne.Analyzers;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Summaries
{
    public class BaseSummary
    {
        protected readonly HtmlResult HtmlResult;

        public string Name { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }

        public BaseSummary(HtmlResult htmlResult)
        {
            HtmlResult = htmlResult;
        }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var currentAssembly = Assembly.GetExecutingAssembly();

            // Get all types within the assembly that are marked with the AnalyzerCategory attribute
            // and have the SummaryName equal to the Name of the current Summary
            var types = currentAssembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, typeof(AnalyzerCategory)) && 
                    Attribute.GetCustomAttributes(x).Any(y => y is AnalyzerCategory && 
                    ((AnalyzerCategory) y).SummaryName == Name));

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
