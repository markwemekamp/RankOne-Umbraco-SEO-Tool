using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Controllers;
using RankOne.Interfaces;
using System;
using System.Web.Http.Results;
using Umbraco.Web;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class AnalysisApiControllerTest : BaseUmbracoTestController
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForAnalyzeService_ThrowsException()
        {
            new AnalysisApiController(null, new Mock<ITypedPublishedContentQuery>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForTypedPublishedContentQuery_ThrowsException()
        {
            new AnalysisApiController(new Mock<IAnalyzeService>().Object, null);
        }

        [TestMethod]
        public void AnalyzeNode_OnExecuteWith0ForId_ReturnsBadRequest()
        {
            var controller = new AnalysisApiController(new Mock<IAnalyzeService>().Object, new Mock<ITypedPublishedContentQuery>().Object);
            var result = controller.AnalyzeNode(0, null);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
