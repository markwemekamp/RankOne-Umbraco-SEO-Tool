using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class AnalyzerStructureTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesCollections()
        {
            var analyzerStructure = new AnalyzerStructure();

            Assert.IsNotNull(analyzerStructure.Analyzers);
        }

        [TestMethod]
        public void AnalyzersProperty_OnSet_SetsTheValue()
        {
            var analyzerStructure = new AnalyzerStructure();
            analyzerStructure.Analyzers = new List<string> { "analyzer" };
            Assert.IsNotNull(analyzerStructure.Analyzers);
            Assert.AreEqual(1, analyzerStructure.Analyzers.Count());
        }

        [TestMethod]
        public void AnalyzersProperty_OnSet_SetsTheValueToNull()
        {
            var analyzerStructure = new AnalyzerStructure();
            analyzerStructure.Analyzers = null;
            Assert.IsNull(analyzerStructure.Analyzers);
        }

        [TestMethod]
        public void NameProperty_OnSet_SetsTheValue()
        {
            var analyzerStructure = new AnalyzerStructure();
            analyzerStructure.Name = "name";
            Assert.AreEqual("name", analyzerStructure.Name);
        }

        [TestMethod]
        public void NameProperty_OnSet_SetsTheValueToNull()
        {
            var analyzerStructure = new AnalyzerStructure();
            analyzerStructure.Name = null;
            Assert.IsNull(analyzerStructure.Name);
        }
    }
}