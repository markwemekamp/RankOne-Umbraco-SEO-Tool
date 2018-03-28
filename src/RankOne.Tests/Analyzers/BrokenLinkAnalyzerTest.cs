using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Services;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class BrokenLinkAnalyzerTest
    {
        private Mock<IUrlStatusService> _urlSstatusServiceMock;
        private BrokenLinkAnalyzer _analyzer;

        [TestInitialize]
        public void Initialize()
        {
            _urlSstatusServiceMock = new Mock<IUrlStatusService>();
            _urlSstatusServiceMock.Setup(x => x.IsActiveUrl("http://www.brokenlink.co.uk.nl")).Returns(false);
            _urlSstatusServiceMock.Setup(x => x.IsActiveUrl("http://www.homepage.com/internal")).Returns(true);

            _analyzer = new BrokenLinkAnalyzer(_urlSstatusServiceMock.Object, new UrlHelper(), new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlStatusService_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(null, new UrlHelper(), new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlHelper_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(new UrlStatusService(RankOneContext.Instance), null, new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForCacheHelper_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(new UrlStatusService(RankOneContext.Instance), new UrlHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            _analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteNoHtml_ReturnsSuccess()
        {
            var document = new HtmlDocument();
            document.LoadHtml("");

            var pageData = new PageData()
            {
                Url = "http://www.google.com",
                Document = document.DocumentNode
            };

            _analyzer.Analyse(pageData);
            var result = _analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("all_links_working", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithBrokenLink_ReturnsWarning()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<a href=\"http://www.brokenlink.co.uk.nl\">link</a>");

            var pageData = new PageData()
            {
                Url = "http://www.google.com",
                Document = document.DocumentNode
            };

            _analyzer.Analyse(pageData);
            var result = _analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("broken_link", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithInternalLink_ReturnsSucess()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<a href=\"/internal\">link</a>");

            var pageData = new PageData()
            {
                Url = "http://www.homepage.com",
                Document = document.DocumentNode
            };

            _analyzer.Analyse(pageData);
            var result = _analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("all_links_working", result.ResultRules.First().Alias);
        }
    }
}