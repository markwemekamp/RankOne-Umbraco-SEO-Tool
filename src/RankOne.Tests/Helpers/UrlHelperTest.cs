using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class UrlHelperTest
    {
        private UrlHelper _urlHelper;

        [TestInitialize]
        public void TestInit()
        {
            _urlHelper = new UrlHelper();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFullPath_OnExecuteWithNullPathParameterForPath_ThrowsException()
        {
            _urlHelper.GetFullPath(null, new Uri("http://www.google.com:80"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFullPath_OnExecuteWithNullPathParameterForUrl_ThrowsException()
        {
            _urlHelper.GetFullPath("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsLocalLink_OnExecuteWithNullUrlParameter_ThrowsException()
        {
            _urlHelper.GetFullPath("/test/test2", null);
        }

        [TestMethod]
        public void GetFullPath_OnExecuteWithRelativePath_ReturnsFullPath()
        {
            var result = _urlHelper.GetFullPath("/test/test2", new Uri("http://www.google.com:443"));

            Assert.AreEqual("http://www.google.com:443/test/test2", result);
        }

        [TestMethod]
        public void GetFullPath_OnExecuteWithFullPath_ReturnsFullPath()
        {
            var result = _urlHelper.GetFullPath("http://localhost:8080/test/test2", new Uri("http://www.google.com:443"));

            Assert.AreEqual("http://localhost:8080/test/test2", result);
        }

        [TestMethod]
        public void GetFullPath_OnExecuteWithRelativePathWithUrlWithoutPort_ReturnsFullPath()
        {
            var result = _urlHelper.GetFullPath("/test/test2", new Uri("http://www.google.com"));

            Assert.AreEqual("http://www.google.com/test/test2", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsLocalLink_OnExecuteWithNullPathParameter_ThrowsException()
        {
            _urlHelper.IsLocalLink(null);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithLocalLink_ReturnsTrue()
        {
            var result = _urlHelper.IsLocalLink("/style.css");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithLocalLinkDotStyleNotation_ReturnsTrue()
        {
            var result = _urlHelper.IsLocalLink("./style.css");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithLocalLinkDotDotStyleNotation_ReturnsTrue()
        {
            var result = _urlHelper.IsLocalLink("../style.css");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithAbsoluteUrl_ReturnsFalse()
        {
            var result = _urlHelper.IsLocalLink("http://www.google.com/style.css");
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContent_OnExecuteWithNullPathParameter_ThrowsException()
        {
            _urlHelper.GetContent(null);
        }
    }
}