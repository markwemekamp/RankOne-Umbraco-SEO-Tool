using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Controllers;
using RankOne.Interfaces;
using System;
using System.Web.Http.Results;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class PageApiControllerTest : BaseUmbracoTestController
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNull_ThrowsException()
        {
            new PageApiController((IPageInformationService)null);
        }

        [TestMethod]
        public void GetPageInformation_OnExecuteWith0ForId_ThrowsException()
        {
            var controller = new PageApiController(new Mock<IPageInformationService>().Object);
            var result = controller.GetPageInformation(0);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
