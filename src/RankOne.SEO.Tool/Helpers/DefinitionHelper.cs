using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Helpers
{
    public class DefinitionHelper : IDefintionHelper
    {
        private IEnumerable<Assembly> _assemblies;

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    _assemblies = new List<Assembly> {currentAssembly};
                }
                return _assemblies;
            }
            set { _assemblies = value; }
        }

        public IEnumerable<SummaryDefinition> GetSummaryDefinitions()
        {
            return Assemblies.SelectMany(GetSummaryDefinitionsFromAssembly);
        }

        public IEnumerable<SummaryDefinition> GetSummaryDefinitionsFromAssembly(Assembly assembly)
        {
            var typesWithSummaryAttribute = assembly.GetTypesWithAttribute(typeof(Summary));

            return typesWithSummaryAttribute.Select(x => new SummaryDefinition
            {
                Type = x,
                Summary = x.GetAttributeWithType<Summary>()
            }).OrderBy(x => x.Summary.SortOrder);
        }

        public IEnumerable<AnalyzerDefinition> GetAnalyzerDefintions()
        {
            return Assemblies.SelectMany(GetAnalyzerDefintionsFromAssembly);
        }

        public IEnumerable<AnalyzerDefinition> GetAnalyzerDefintionsFromAssembly(Assembly assembly)
        {
            var typesWithAnalyzerCategoryAttribute = assembly.GetTypesWithAttribute(typeof(AnalyzerCategory));

            return typesWithAnalyzerCategoryAttribute.Select(x => new AnalyzerDefinition
            {
                Type = x,
                AnalyzerCategory = x.GetAttributeWithType<AnalyzerCategory>()
            });
        }

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the current assembly
        /// </summary>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        public IEnumerable<Type> GetAllAnalyzerTypesForSummary(string summaryName)
        {
            return Assemblies.SelectMany(a => GetAllAnalyzerTypesForSummary(a, summaryName));
        }

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the given assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        public IEnumerable<Type> GetAllAnalyzerTypesForSummary(Assembly assembly, string summaryName)
        {
            return GetAnalyzerDefintionsFromAssembly(assembly).
                Where(x => x.AnalyzerCategory.SummaryName == summaryName).
                Select(x => x.Type);
        }
    }
}
