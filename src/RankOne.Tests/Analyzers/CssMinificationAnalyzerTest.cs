using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class CssMinificationAnalyzerTest
    {
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
            document.LoadHtml("<link href=\"../../files/unminified.css\" rel=\"stylesheet\" />");

            var pageData = new PageData()
            {
                Document = document.DocumentNode
            };

            var analyzer = new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), new UrlHelper());
            analyzer.Analyse(pageData);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithUnminifiedCss_ReturnsHint()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<link href=\"../../files/unminified.css\" rel=\"stylesheet\" />");

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
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("file_not_minified", result.ResultRules.First().Alias);
            Assert.AreEqual("../../files/unminified.css", result.ResultRules.First().Tokens.First());
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMinifiedCss_ReturnsSuccess()
        {
            var document = new HtmlDocument();
            document.LoadHtml("<link href=\"../../files/minified.css\" rel=\"stylesheet\" />");

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
