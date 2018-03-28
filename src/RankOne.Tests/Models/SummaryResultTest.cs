using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class SummaryResultTest
    {
        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValue()
        {
            var summaryResult = new SummaryResult();
            summaryResult.Alias = "alias";
            Assert.AreEqual("alias", summaryResult.Alias);
        }

        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValueToNull()
        {
            var summaryResult = new SummaryResult();
            summaryResult.Alias = null;
            Assert.IsNull(summaryResult.Alias);
        }

        [TestMethod]
        public void AnalysisProperty_OnSet_SetsTheValue()
        {
            var summaryResult = new SummaryResult();
            summaryResult.Analysis = new Analysis();
            Assert.IsNotNull(summaryResult.Analysis);
        }

        [TestMethod]
        public void AnalysisProperty_OnSet_SetsTheValueToNull()
        {
            var summaryResult = new SummaryResult();
            summaryResult.Analysis = null;
            Assert.IsNull(summaryResult.Analysis);
        }

        [TestMethod]
        public void ErrorCountProperty_OnGet_GetsTheNumberOfErrors()
        {
            var summaryResult = new SummaryResult
            {
                Analysis = new Analysis()
                {
                    Results = new List<AnalyzeResult>() {
                    new AnalyzeResult() {
                        ResultRules = new List<ResultRule>() {
                            new ResultRule() { Alias = "rule 1", Type = ResultType.Error },
                            new ResultRule() { Alias = "rule 2", Type = ResultType.Error }
                        }
                    }
                }
                }
            };
            var result = summaryResult.ErrorCount;
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void WarningCountProperty_OnGet_GetsTheNumberOfWarnings()
        {
            var summaryResult = new SummaryResult
            {
                Analysis = new Analysis()
                {
                    Results = new List<AnalyzeResult>() {
                    new AnalyzeResult() {
                        ResultRules = new List<ResultRule>() {
                            new ResultRule() { Alias = "rule 1", Type = ResultType.Warning },
                            new ResultRule() { Alias = "rule 2", Type = ResultType.Warning }
                        }
                    }
                }
                }
            };
            var result = summaryResult.WarningCount;
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void HintCountProperty_OnGet_GetsTheNumberOfHints()
        {
            var summaryResult = new SummaryResult
            {
                Analysis = new Analysis()
                {
                    Results = new List<AnalyzeResult>() {
                    new AnalyzeResult() {
                        ResultRules = new List<ResultRule>() {
                            new ResultRule() { Alias = "rule 1", Type = ResultType.Hint },
                            new ResultRule() { Alias = "rule 2", Type = ResultType.Hint }
                        }
                    }
                }
                }
            };
            var result = summaryResult.HintCount;
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void SuccessCountProperty_OnGet_GetsTheNumberOfSuccesses()
        {
            var summaryResult = new SummaryResult
            {
                Analysis = new Analysis()
                {
                    Results = new List<AnalyzeResult>() {
                    new AnalyzeResult() {
                        ResultRules = new List<ResultRule>() {
                            new ResultRule() { Alias = "rule 1", Type = ResultType.Success },
                            new ResultRule() { Alias = "rule 2", Type = ResultType.Success }
                        }
                    }
                }
                }
            };
            var result = summaryResult.SuccessCount;
            Assert.AreEqual(2, result);
        }
    }
}