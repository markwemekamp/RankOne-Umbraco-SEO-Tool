using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class PageScoreTest
    {
        [TestMethod]
        public void OverallScoreProperty_OnSet_SetsTheValue()
        {
            var pageScore = new PageScore();
            pageScore.OverallScore = 100;

            Assert.AreEqual(100, pageScore.OverallScore);
        }

        [TestMethod]
        public void ErrorCountProperty_OnSet_SetsTheValue()
        {
            var pageScore = new PageScore();
            pageScore.ErrorCount = 100;

            Assert.AreEqual(100, pageScore.ErrorCount);
        }

        [TestMethod]
        public void WarningCountProperty_OnSet_SetsTheValue()
        {
            var pageScore = new PageScore();
            pageScore.WarningCount = 100;

            Assert.AreEqual(100, pageScore.WarningCount);
        }

        [TestMethod]
        public void HintCountProperty_OnSet_SetsTheValue()
        {
            var pageScore = new PageScore();
            pageScore.HintCount = 100;

            Assert.AreEqual(100, pageScore.HintCount);
        }

        [TestMethod]
        public void SuccessCountProperty_OnSet_SetsTheValue()
        {
            var pageScore = new PageScore();
            pageScore.SuccessCount = 100;

            Assert.AreEqual(100, pageScore.SuccessCount);
        }
    }
}
