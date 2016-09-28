using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RankOne.Attributes;
using RankOne.Models;
using RankOne.Summaries;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    public class AnalyzerStructureApiController : UmbracoAuthorizedApiController
    {
        public IEnumerable<AnalyzerStructure> GetStructure()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var summaries = currentAssembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, typeof(Summary))).Select(x => new
                    {
                        Type = x,
                        Summary = (Summary)Attribute.GetCustomAttributes(x).FirstOrDefault(y => y is Summary)
                    }).OrderBy(x => x.Summary.SortOrder);
            var analyzers = currentAssembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, typeof(AnalyzerCategory))).Select(x => new
                    {
                        Type = x,
                        AnalyzerCategory =
                            (AnalyzerCategory)
                                Attribute.GetCustomAttributes(x).FirstOrDefault(y => y is AnalyzerCategory)
                    });


            var structure = new List<AnalyzerStructure>();
            foreach (var summary in summaries)
            {
                var summaryInstance = Activator.CreateInstance(summary.Type);
                structure.Add(new AnalyzerStructure
                {
                    Name = summary.Summary.Alias,
                    Analyzers = analyzers.Where(x => x.AnalyzerCategory.SummaryName == ((BaseSummary)summaryInstance).Name)
                                    .Select(x => x.AnalyzerCategory.Alias)
                });

            }
            return structure;
        }
    }
}
