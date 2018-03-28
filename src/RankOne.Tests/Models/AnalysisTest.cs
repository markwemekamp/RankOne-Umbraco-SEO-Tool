using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class AnalysisTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesCollections()
        {
            var analysis = new Analysis();

            Assert.IsNotNull(analysis.Information);
            Assert.IsNotNull(analysis.Results);
        }

        [TestMethod]
        public void InformationProperty_OnSet_SetsTheValue()
        {
            var analysis = new Analysis();
            analysis.Information = new List<AnalysisInformation>() { new AnalysisInformation() };
            Assert.IsNotNull(analysis.Information);
            Assert.AreEqual(1, analysis.Information.Count());
        }

        [TestMethod]
        public void InformationProperty_OnSet_SetsTheValueToNull()
        {
            var analysis = new Analysis();
            analysis.Information = null;
            Assert.IsNull(analysis.Information);
        }

        [TestMethod]
        public void ResultsProperty_OnSet_SetsTheValue()
        {
            var analysis = new Analysis();
            analysis.Results = new List<AnalyzeResult>() { new AnalyzeResult() };
            Assert.IsNotNull(analysis.Results);
            Assert.AreEqual(1, analysis.Results.Count());
        }

        [TestMethod]
        public void ResultsProperty_OnSet_SetsTheValueToNull()
        {
            var analysis = new Analysis();
            analysis.Results = null;
            Assert.IsNull(analysis.Results);
        }
    }
}