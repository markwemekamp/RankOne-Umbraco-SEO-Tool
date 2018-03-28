using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class GZipAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForEncodingHelper_ThrowArgumentNullException()
        {
            new GZipAnalyzer((IEncodingHelper)null, new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForCacheHelper_ThrowArgumentNullException()
        {
            new GZipAnalyzer(new EncodingHelper(), (ICacheHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new GZipAnalyzer(new EncodingHelper(), new CacheHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoGzipSupport_SetsResult()
        {
            var mockEncodingHelper = new Mock<IEncodingHelper>();
            mockEncodingHelper.Setup(x => x.GetEncodingByUrl("http://www.google.nl")).Returns("");

            var analyzer = new GZipAnalyzer(mockEncodingHelper.Object, new CacheHelper());

            var pageData = new PageData()
            {
                Url = "http://www.google.nl"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("gzip_disabled", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithGzipSupport_SetsResult()
        {
            var mockEncodingHelper = new Mock<IEncodingHelper>();
            mockEncodingHelper.Setup(x => x.GetEncodingByUrl("http://www.google.nl")).Returns("gzip");

            var analyzer = new GZipAnalyzer(mockEncodingHelper.Object, new CacheHelper());

            var pageData = new PageData()
            {
                Url = "http://www.google.nl"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("gzip_enabled", result.ResultRules.First().Alias);
        }
    }
}