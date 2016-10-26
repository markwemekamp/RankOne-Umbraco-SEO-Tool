using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Services
{
    public class ReflectionService
    {
        public IEnumerable<SummaryDefinition> GetSummaries()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            return GetSummaries(currentAssembly);
        }
        public IEnumerable<SummaryDefinition> GetSummaries(Assembly assembly)
        {
            var typesWithSummaryAttribute = assembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, typeof(Summary)));

            return typesWithSummaryAttribute.Select(x => new SummaryDefinition
            {
                Type = x,
                Summary = (Summary)Attribute.GetCustomAttributes(x).FirstOrDefault(y => y is Summary)
            }).OrderBy(x => x.Summary.SortOrder);
        }

        public IEnumerable<AnalyzerDefinition> GetAnalyzers()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            return GetAnalyzers(currentAssembly);
        }
        public IEnumerable<AnalyzerDefinition> GetAnalyzers(Assembly assembly)
        {
            var typesWithAnalyzerCategoryAttribute = assembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, typeof(AnalyzerCategory)));

            return typesWithAnalyzerCategoryAttribute.Select(x => new AnalyzerDefinition
            {
                Type = x,
                AnalyzerCategory =
                            (AnalyzerCategory)
                                Attribute.GetCustomAttributes(x).FirstOrDefault(y => y is AnalyzerCategory)
            });
        }

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the current assembly
        /// </summary>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        public IEnumerable<Type> GetAllAnalyzersForSummary(string summaryName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            return GetAllAnalyzersForSummary(currentAssembly, summaryName);
        }

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the given assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        public IEnumerable<Type> GetAllAnalyzersForSummary(Assembly assembly, string summaryName)
        {
            return GetAnalyzers(assembly).Where(x => x.AnalyzerCategory.SummaryName == summaryName).Select(x => x.Type);
        }
    }
}
