using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RankOne.Attributes;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Helpers
{
    public class DefintionHelper : IDefintionHelper
    {
        private readonly IReflectionHelper _reflectionHelper;
        private IEnumerable<Assembly> _assemblies;

        public DefintionHelper() : this(new ReflectionHelper())
        { }

        public DefintionHelper(IReflectionHelper reflectionHelper)
        {
            _reflectionHelper = reflectionHelper;
            Initialize();
        }

        protected void Initialize()
        {
            _assemblies = _reflectionHelper.GetAssemblies();
        }

        public IEnumerable<SummaryDefinition> GetSummaryDefinitions()
        {
            return _assemblies.SelectMany(GetSummaryDefinitionsFromAssembly);
        }

        public IEnumerable<SummaryDefinition> GetSummaryDefinitionsFromAssembly(Assembly assembly)
        {
            var typesWithSummaryAttribute = _reflectionHelper.GetTypesWithAttribute(assembly, typeof(Summary));

            return typesWithSummaryAttribute.Select(x => new SummaryDefinition
            {
                Type = x,
                Summary = _reflectionHelper.GetAttributeFromType<Summary>(x)
            }).OrderBy(x => x.Summary.SortOrder);
        }

        public IEnumerable<AnalyzerDefinition> GetAnalyzerDefintions()
        {
            return _assemblies.SelectMany(GetAnalyzerDefintionsFromAssembly);
        }

        public IEnumerable<AnalyzerDefinition> GetAnalyzerDefintionsFromAssembly(Assembly assembly)
        {
            var typesWithAnalyzerCategoryAttribute = _reflectionHelper.GetTypesWithAttribute(assembly, typeof(AnalyzerCategory));

            return typesWithAnalyzerCategoryAttribute.Select(x => new AnalyzerDefinition
            {
                Type = x,
                AnalyzerCategory = _reflectionHelper.GetAttributeFromType<AnalyzerCategory>(x)
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
            return _assemblies.SelectMany(a => GetAllAnalyzerTypesForSummary(a, summaryName));
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
