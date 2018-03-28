using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Services;
using System;

namespace RankOne.Tests.Services
{
    [TestClass]
    public class UrlStatusServiceTest
    {
        private Mock<IWebRequestHelper> _webRequestHelperMock;
        private UrlStatusService _urlStatusService;

        [TestInitialize]
        public void Initialize()
        {
            _webRequestHelperMock = new Mock<IWebRequestHelper>();
            _webRequestHelperMock.Setup(x => x.IsActiveUrl("http://www.google.com")).Returns(true);
            _webRequestHelperMock.Setup(x => x.IsActiveUrl("http://www.googel.com")).Returns(false);

            _urlStatusService = new UrlStatusService(_webRequestHelperMock.Object, new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForWebrequestHelper_ThrowsException()
        {
            new UrlStatusService(null, new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForCacheHelper_ThrowsException()
        {
            new UrlStatusService(_webRequestHelperMock.Object, null);
        }

        [TestMethod]
        public void IsActiveUrl_OnExecuteWithActiveUrl_GetsStatusOfUrl()
        {
            var result = _urlStatusService.IsActiveUrl("http://www.google.com");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsActiveUrl_OnExecuteWithInactiveUrl_GetsStatusOfUrl()
        {
            var result = _urlStatusService.IsActiveUrl("http://www.googel.com");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsActiveUrl_OnExecuteWithMultipleCalls_ReturnsCachedValue()
        {
            _urlStatusService.IsActiveUrl("http://www.google.com");
            _urlStatusService.IsActiveUrl("http://www.google.com");
            _urlStatusService.IsActiveUrl("http://www.google.com");
            _urlStatusService.IsActiveUrl("http://www.google.com");
            _urlStatusService.IsActiveUrl("http://www.google.com");
            _urlStatusService.IsActiveUrl("http://www.google.com");

            _webRequestHelperMock.Verify(x => x.IsActiveUrl("http://www.google.com"), Times.AtMostOnce);
        }
    }
}