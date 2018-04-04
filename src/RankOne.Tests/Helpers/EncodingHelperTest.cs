using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;
using System.Net;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class EncodingHelperTest
    {
        private EncodingHelper _encodingHelper;

        [TestInitialize]
        public void TestInit()
        {
            _encodingHelper = new EncodingHelper();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowsException()
        {
            new EncodingHelper((IHttpWebRequestFactory)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEncodingByUrl_OnExecuteWithNullParameter_ThrowsException()
        {
            _encodingHelper.GetEncodingByUrl(null);
        }

        [TestMethod]
        public void Test()
        {
            var response = new Mock<HttpWebResponse>();
            response.Setup(x => x.ContentEncoding).Returns("gzip");

            var request = new Mock<HttpWebRequest>();
            request.Setup(x => x.GetResponse()).Returns(response.Object);
            request.Setup(x => x.Headers).Returns(new WebHeaderCollection());

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(x => x.Create(It.IsAny<string>())).Returns(request.Object);

            var encodingHelper = new EncodingHelper(factory.Object);

            encodingHelper.GetEncodingByUrl("http://www.test.com");

            request.Verify(x => x.Method == "Get");
            request.Verify(x => x.Headers.Count >= 1);
            request.Verify(x => x.Headers["Accept-Encoding"] == "gzip,deflate");
        }
    }
}