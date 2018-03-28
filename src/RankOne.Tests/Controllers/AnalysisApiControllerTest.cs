using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Controllers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Tests.Mock;
using System;
using System.Web.Http.Results;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class AnalysisApiControllerTest : BaseUmbracoControllerTest
    {
        private Mock<ITypedPublishedContentQuery> _typedPublishedContentQueryMock;
        private Mock<IAnalyzeService> _analyzeServiceMock;
        private AnalysisApiController _controllerMock;

        [TestInitialize]
        public void Initialize()
        {
            _typedPublishedContentQueryMock = new Mock<ITypedPublishedContentQuery>();
            _typedPublishedContentQueryMock.Setup(x => x.TypedContent(10)).Throws(new MissingFieldException());
            _typedPublishedContentQueryMock.Setup(x => x.TypedContent(20)).Throws(new Exception());
            _typedPublishedContentQueryMock.Setup(x => x.TypedContent(30)).Returns(new PublishedContentMock());

            _analyzeServiceMock = new Mock<IAnalyzeService>();
            _controllerMock = new AnalysisApiController(_analyzeServiceMock.Object, _typedPublishedContentQueryMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForAnalyzeService_ThrowsException()
        {
            new AnalysisApiController(null, _typedPublishedContentQueryMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForTypedPublishedContentQuery_ThrowsException()
        {
            new AnalysisApiController(_analyzeServiceMock.Object, null);
        }

        [TestMethod]
        public void AnalyzeNode_OnExecuteWith0ForId_ReturnsBadRequest()
        {
            var result = _controllerMock.AnalyzeNode(0, null);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void AnalyzeNode_OnExecuteWithMissingFieldException_ReturnsInternalServerError()
        {
            var result = _controllerMock.AnalyzeNode(10, null);

            _typedPublishedContentQueryMock.Verify(x => x.TypedContent(10));
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));
        }

        [TestMethod]
        public void AnalyzeNode_OnExecuteWithGeneralException_ReturnsInternalServerError()
        {
            var result = _controllerMock.AnalyzeNode(20, null);

            _typedPublishedContentQueryMock.Verify(x => x.TypedContent(20));
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));
        }

        [TestMethod]
        public void AnalyzeNode_OnExecute_ReturnsOk()
        {
            var result = _controllerMock.AnalyzeNode(30, null);

            _typedPublishedContentQueryMock.Verify(x => x.TypedContent(30));
            _analyzeServiceMock.Verify(x => x.CreateAnalysis(It.IsAny<IPublishedContent>(), null), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PageAnalysis>));
        }
    }
}