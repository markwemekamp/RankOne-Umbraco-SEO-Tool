using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RankOne.Interfaces
{
    public interface IDefintionHelper
    {
        IEnumerable<SummaryDefinition> GetSummaryDefinitions();

        IEnumerable<SummaryDefinition> GetSummaryDefinitionsFromAssembly(Assembly assembly);

        IEnumerable<AnalyzerDefinition> GetAnalyzerDefintions();

        IEnumerable<AnalyzerDefinition> GetAnalyzerDefintionsFromAssembly(Assembly assembly);

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the current assembly
        /// </summary>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        IEnumerable<Type> GetAllAnalyzerTypesForSummary(string summaryName);

        /// <summary>
        /// Gets all types within the assembly that are marked with the AnalyzerCategory attribute
        /// and have the SummaryName equal to the given summaryName of the given assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns></returns>
        IEnumerable<Type> GetAllAnalyzerTypesForSummary(Assembly assembly, string summaryName);
    }
}