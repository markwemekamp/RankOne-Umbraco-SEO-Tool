using System;
using System.Collections.Generic;
using System.Linq;
using RankOne.Models;
using RankOne.Services;
using RankOne.Summaries;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class AnalyzerStructureApiController : UmbracoAuthorizedApiController
    {
        public IEnumerable<AnalyzerStructure> GetStructure()
        {
            var reflectionService = new ReflectionService();
            var summaries = reflectionService.GetSummaries();
            var analyzers = reflectionService.GetAnalyzers();

            var structure = new List<AnalyzerStructure>();
            foreach (var summary in summaries)
            {
                var summaryInstance = Activator.CreateInstance(summary.Type);
                var analyzersForSummary = analyzers.Where(
                    x => x.AnalyzerCategory.SummaryName == ((BaseSummary) summaryInstance).Name)
                    .Select(x => x.AnalyzerCategory.Alias);

                structure.Add(new AnalyzerStructure
                {
                    Name = summary.Summary.Alias,
                    Analyzers = analyzersForSummary
                });

            }
            return structure;
        }
    }
}
