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
        public void IsLocalLink_OnExecuteWithNullPathParameter_ThrowsException()
        {
            _urlHelper.GetFullPath(null, new Uri("http://www.google.com:80"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsLocalLink_OnExecuteWithNullUrlParameter_ThrowsException()
        {
            _urlHelper.GetFullPath("/test/test2", null);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithRelativePath_ReturnsFullPath()
        {
            var result = _urlHelper.GetFullPath("/test/test2", new Uri("http://www.google.com:80"));

            Assert.AreEqual("http://www.google.com:80/test/test2", result);
        }

        [TestMethod]
        public void IsLocalLink_OnExecuteWithFullPath_ReturnsFullPath()
        {
            var result = _urlHelper.GetFullPath("http://localhost:8080/test/test2", new Uri("http://www.google.com:80"));

            Assert.AreEqual("http://localhost:8080/test/test2", result);
        }
    }
}
