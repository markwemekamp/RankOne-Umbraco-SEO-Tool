using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Serializers;
using RankOne.Services;
using System;
using Umbraco.Web;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class PageScoreNodeHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForTypedPublishedContentQuery_ThrowsException()
        {
            new PageScoreNodeHelper(null, new Mock<INodeReportService>().Object, new PageScoreSerializer(), new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForNodeReportService_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, null, new PageScoreSerializer(), new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForPageScoreSerializer_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, new Mock<INodeReportService>().Object, null, new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForAnalyzeService_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, new Mock<INodeReportService>().Object, new PageScoreSerializer(), null);
        }
    }
}
