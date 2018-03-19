
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class AnalysisInformationTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesCollections()
        {
            var analysisInformation = new AnalysisInformation();
            Assert.IsNotNull(analysisInformation.Tokens);
        }

        [TestMethod]
        public void TokensProperty_OnSet_SetsTheValue()
        {
            var analysisInformation = new AnalysisInformation();
            analysisInformation.Tokens = new List<string>() { "token" };
            Assert.IsNotNull(analysisInformation.Tokens);
            Assert.AreEqual(1, analysisInformation.Tokens.Count());
        }

        [TestMethod]
        public void TokensProperty_OnSet_SetsTheValueToNull()
        {
            var analysisInformation = new AnalysisInformation();
            analysisInformation.Tokens = null;
            Assert.IsNull(analysisInformation.Tokens);
        }

        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValue()
        {
            var analysisInformation = new AnalysisInformation();
            analysisInformation.Alias = "alias";
            Assert.AreEqual("alias", analysisInformation.Alias);
        }

        [TestMethod]
        public void ResultsProperty_OnSet_SetsTheValueToNull()
        {
            var analysisInformation = new AnalysisInformation();
            analysisInformation.Alias = null;
            Assert.IsNull(analysisInformation.Alias);
        }
    }
}
