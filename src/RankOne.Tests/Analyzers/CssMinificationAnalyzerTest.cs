using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.IO;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class CssMinificationAnalyzerTest
    {
        public CssMinificationAnalyzer GetAnalyzer()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.GetFullPath("/files/unminified.css", It.IsAny<Uri>())).Returns("/files/unminified.css");
            mockUrlHelper.Setup(x => x.GetFullPath("/files/minified.css", It.IsAny<Uri>())).Returns("/files/minified.css");
            mockUrlHelper.Setup(x => x.GetContent("/files/unminified.css")).Returns(File.ReadAllText("./files/unminified.css"));
            mockUrlHelper.Setup(x => x.GetContent("/files/minified.css")).Returns(File.ReadAllText("./files/minified.css"));
            return new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), mockUrlHelper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForMinificationHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(null, new CacheHelper(), new UrlHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForCacheHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(new MinificationHelper(), null, new UrlHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), new UrlHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithPageUrlSetToNull_ThrowsException()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<link href=\"files/unminified.css\" rel=\"stylesheet\" />");

            var pageData = new PageData()
            {
                Document = document.DocumentNode
            };

            var analyzer = new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), new UrlHelper());
            analyzer.Analyse(pageData);
        }

        [TestMethod]
        [DeploymentItem("../../files/unminified.css", "files")]
        public void Analyse_OnExecuteWithUnminifiedCss_ReturnsHint()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<link href=\"/files/unminified.css\" rel=\"stylesheet\" />");

            var pageData = new PageData()
            {
                Document = document.DocumentNode,
                Url = "http://www.google.nl/"
            };

            var analyzer = GetAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("file_not_minified", result.ResultRules.First().Alias);
            Assert.AreEqual("/files/unminified.css", result.ResultRules.First().Tokens.First());
        }

        [TestMethod]
        [DeploymentItem("../../files/minified.css", "files")]
        public void Analyse_OnExecuteWithMinifiedCss_ReturnsSuccess()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<link href=\"/files/minified.css\" rel=\"stylesheet\" />");

            var pageData = new PageData()
            {
                Document = document.DocumentNode,
                Url = "http://www.google.nl/"
            };

            var analyzer = GetAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("all_minified", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoDocuments_ReturnsSuccess()
        {
            var document = new HtmlDocument();
            document.LoadHtml("");

            var pageData = new PageData()
            {
                Document = document.DocumentNode,
                Url = "http://www.google.nl/"
            };

            var analyzer = new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), new UrlHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("all_minified", result.ResultRules.First().Alias);
        }
    }
}
