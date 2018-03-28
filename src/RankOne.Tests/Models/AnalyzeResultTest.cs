using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class AnalyzeResultTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesCollections()
        {
            var analyzeResult = new AnalyzeResult();

            Assert.IsNotNull(analyzeResult.ResultRules);
        }

        [TestMethod]
        public void ResultRulesProperty_OnSet_SetsTheValue()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.ResultRules = new List<ResultRule> { new ResultRule() };
            Assert.IsNotNull(analyzeResult.ResultRules);
            Assert.AreEqual(1, analyzeResult.ResultRules.Count());
        }

        [TestMethod]
        public void ResultRulesProperty_OnSetWithNull_SetsTheValueToNull()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.ResultRules = null;
            Assert.IsNull(analyzeResult.ResultRules);
        }

        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValue()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.Alias = "alias";
            Assert.IsNotNull(analyzeResult.Alias);
            Assert.AreEqual("alias", analyzeResult.Alias);
        }

        [TestMethod]
        public void AliasProperty_OnSetWithNull_SetsTheValueToNull()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.Alias = null;
            Assert.IsNull(analyzeResult.Alias);
        }

        [TestMethod]
        public void WeightProperty_OnSet_SetsTheValue()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.Weight = 5;
            Assert.AreEqual(5, analyzeResult.Weight);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddResultRule_OnExecuteWithNullForCode_ThrowsException()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.AddResultRule(null, "type");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddResultRule_OnExecuteWithNullForType_ThrowsException()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.AddResultRule("code", null);
        }

        [TestMethod]
        public void AddResultRule_OnExecute_SetsTheValue()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.AddResultRule("code", "type");
            Assert.IsNotNull(analyzeResult.ResultRules);
            Assert.IsTrue(analyzeResult.ResultRules.Any());
            Assert.IsTrue(analyzeResult.ResultRules.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountRestultRulesByType_OnExecuteWithNull_ThrowsException()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.CountRestultRulesByType(null);
        }

        [TestMethod]
        public void CountRestultRulesByType_OnExecute_ReturnsNumberOfResultsWithType()
        {
            var analyzeResult = new AnalyzeResult();
            analyzeResult.AddResultRule("code", "type1");
            analyzeResult.AddResultRule("code", "type1");
            analyzeResult.AddResultRule("code", "type2");
            analyzeResult.AddResultRule("code", "type2");
            analyzeResult.AddResultRule("code", "type2");

            var type1Counter = analyzeResult.CountRestultRulesByType("type1");
            var type2Counter = analyzeResult.CountRestultRulesByType("type2");
            var type3Counter = analyzeResult.CountRestultRulesByType("type3");

            Assert.AreEqual(2, type1Counter);
            Assert.AreEqual(3, type2Counter);
            Assert.AreEqual(0, type3Counter);
        }

        [TestMethod]
        public void ErrorCountProperty_OnGet_ReturnsNumberOfResultsWithError()
        {
            var analyzeResult = new AnalyzeResult();
            var result = analyzeResult.ErrorCount;
            Assert.AreEqual(0, result);
            analyzeResult.AddResultRule("code", ResultType.Error);
            var result2 = analyzeResult.ErrorCount;
            Assert.AreEqual(1, result2);
        }

        [TestMethod]
        public void WarningCountProperty_OnGet_ReturnsNumberOfResultsWithError()
        {
            var analyzeResult = new AnalyzeResult();
            var result = analyzeResult.WarningCount;
            Assert.AreEqual(0, result);
            analyzeResult.AddResultRule("code", ResultType.Warning);
            var result2 = analyzeResult.WarningCount;
            Assert.AreEqual(1, result2);
        }

        [TestMethod]
        public void HintCountProperty_OnGet_ReturnsNumberOfResultsWithError()
        {
            var analyzeResult = new AnalyzeResult();
            var result = analyzeResult.HintCount;
            Assert.AreEqual(0, result);
            analyzeResult.AddResultRule("code", ResultType.Hint);
            var result2 = analyzeResult.HintCount;
            Assert.AreEqual(1, result2);
        }

        [TestMethod]
        public void SuccessCountProperty_OnGet_ReturnsNumberOfResultsWithError()
        {
            var analyzeResult = new AnalyzeResult();
            var result = analyzeResult.SuccessCount;
            Assert.AreEqual(0, result);
            analyzeResult.AddResultRule("code", ResultType.Success);
            var result2 = analyzeResult.SuccessCount;
            Assert.AreEqual(1, result2);
        }
    }
}