using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class BaseAnalyzerTest
    {
        [TestMethod]
        public void Constructor_OnExecute_SetsAnalyzeResult()
        {
            var randomAnalyzer = new KeywordHeadingAnalyzer();
            Assert.IsNotNull(randomAnalyzer.AnalyzeResult);
        }

        [TestMethod]
        public void Weight_OnSet_SetsWeightOfAnalyzeResult()
        {
            var randomAnalyzer = new KeywordHeadingAnalyzer();
            Assert.AreEqual(0, randomAnalyzer.Weight);
            Assert.AreEqual(100, randomAnalyzer.AnalyzeResult.Weight); // default value
            randomAnalyzer.Weight = 55;
            Assert.AreEqual(55, randomAnalyzer.Weight);
            Assert.AreEqual(55, randomAnalyzer.AnalyzeResult.Weight);
        }

        [TestMethod]
        public void Alias_OnSet_SetsAliasOfAnalyzeResult()
        {
            var randomAnalyzer = new KeywordHeadingAnalyzer();
            Assert.AreEqual(null, randomAnalyzer.Alias);
            Assert.AreEqual(null, randomAnalyzer.AnalyzeResult.Alias);
            randomAnalyzer.Alias = "test";
            Assert.AreEqual("test", randomAnalyzer.Alias);
            Assert.AreEqual("test", randomAnalyzer.AnalyzeResult.Alias);
        }
    }
}
