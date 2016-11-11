using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class DefinitionHelperTests
    {
        [TestMethod]
        public void SettingAssembliesProperty()
        {
            var rankOneAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "RankOne");

            var assemblyCollection = new List<Assembly> {rankOneAssembly};

            var definitionHelper = new DefinitionHelper {Assemblies = assemblyCollection};

            Assert.IsNotNull(definitionHelper.Assemblies);
            Assert.IsNotNull(definitionHelper.Assemblies.Any());
            Assert.IsTrue(definitionHelper.Assemblies.Count() == 1);
            Assert.AreEqual(rankOneAssembly, definitionHelper.Assemblies.First());
        }

        [TestMethod]
        public void RankOneContainsSummaryTypes()
        {
            var rankOneAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "RankOne");

            var definitionHelper = new DefinitionHelper();

            var summaryDefinitions = definitionHelper.GetSummaryDefinitionsFromAssembly(rankOneAssembly);

            Assert.IsNotNull(summaryDefinitions);
            Assert.IsTrue(summaryDefinitions.Any());
        }

        [TestMethod]
        public void RankOneContainsAnalyzerTypes()
        {
            var rankOneAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "RankOne");

            var definitionHelper = new DefinitionHelper();

            var analyzerDefinitions = definitionHelper.GetAnalyzerDefintionsFromAssembly(rankOneAssembly);

            Assert.IsNotNull(analyzerDefinitions);
            Assert.IsTrue(analyzerDefinitions.Any());
        }
    }
}
