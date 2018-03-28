using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Summaries;
using RankOne.Tests.Mocks;
using System.Collections.Generic;

namespace RankOne.Tests.Summaries
{
    [TestClass]
    public class BaseSummaryTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesAnalyzersCollection()
        {
            var summary = new BaseSummary();

            Assert.IsNotNull(summary.Analyzers);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithNoAnalyzers_ReturnsAnalysisObject()
        {
            var summary = new BaseSummary();
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Analysis));
        }

        [TestMethod]
        public void GetAnalysis_OnExecute_ReturnsAnalysisObject()
        {
            var analyzerMock = new Mock<IAnalyzer>();

            var summary = new BaseSummary()
            {
                Analyzers = new List<IAnalyzer>()
                {
                   analyzerMock.Object
                }
            };
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Analysis));
            analyzerMock.Verify(x => x.Analyse(It.IsAny<PageData>()), Times.Once);
        }
    }
}