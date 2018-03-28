using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class KeywordHeadingAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInH1_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><h1>focus</h1></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("keyword_used_in_heading", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInH2_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><h2>focus</h2></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("keyword_used_in_heading", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInH3_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><h3>focus</h3></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("keyword_used_in_heading", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInH4_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><h4>focus</h4></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("keyword_used_in_heading", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInH5_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<body><h5>focus</h5></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.IsTrue(!result.ResultRules.First().Tokens.Any());
            Assert.AreEqual("keyword_not_used_in_heading", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentInMultipleHeadingTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<h1>focus</h1><h2>focus</h2><h3>focus</h3><h4>focus</h4>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordHeadingAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("4", result.ResultRules.First().Tokens.First());
            Assert.AreEqual("keyword_used_in_heading", result.ResultRules.First().Alias);
        }
    }
}