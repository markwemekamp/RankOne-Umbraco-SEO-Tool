using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Controllers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Web.Http.Results;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class PageApiControllerTest : BaseUmbracoControllerTest
    {
        private Mock<IPageInformationService> _pageInformationService;
        private PageApiController _controllerMock;

        [TestInitialize]
        public void Initialize()
        {
            _pageInformationService = new Mock<IPageInformationService>();
            _pageInformationService.Setup(x => x.GetpageInformation(1)).Throws(new Exception());
            _pageInformationService.Setup(x => x.GetpageInformation(2)).Returns(new PageInformation());

            _controllerMock = new PageApiController(_pageInformationService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNull_ThrowsException()
        {
            new PageApiController((IPageInformationService)null);
        }

        [TestMethod]
        public void GetPageInformation_OnExecuteWith0ForId_ThrowsException()
        {
            var result = _controllerMock.GetPageInformation(0);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetPageInformation_OnExecuteWithException_ReturnsInternalServerError()
        {
            var result = _controllerMock.GetPageInformation(1);

            _pageInformationService.Verify(x => x.GetpageInformation(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));
        }

        [TestMethod]
        public void GetPageInformation_OnExecute_ReturnsOk()
        {
            var result = _controllerMock.GetPageInformation(2);

            _pageInformationService.Verify(x => x.GetpageInformation(2), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PageInformation>));
        }
    }
}