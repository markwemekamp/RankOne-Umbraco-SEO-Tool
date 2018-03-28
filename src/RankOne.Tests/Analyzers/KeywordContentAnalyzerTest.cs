using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class KeywordContentAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordContentAnalyzer((IHtmlTagHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><div>focus</div></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("content_contains_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordsPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><div>focus</div><div>focus</div><div>focus</div><div>focus</div></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("4", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("content_contains_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordNotPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><div></div></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.IsTrue(!result.ResultRules.First().Tokens.Any());
            Assert.AreEqual("content_doesnt_contain_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoBodyTag_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div>focus</div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_body_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleBodyTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><div><body>focus</body></div></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_body_tags", result.ResultRules.First().Alias);
        }
    }
}