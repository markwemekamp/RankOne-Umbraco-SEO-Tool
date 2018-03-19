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
    public class KeywordUrlAnalyzerTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordUrlAnalyzer();
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            var pageData = new PageData()
            {
                Focuskeyword = "focus",
                Url = "http://localhost/focus/"
            };

            var analyzer = new KeywordUrlAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("url_contains_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordNotPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();

            var pageData = new PageData()
            {
                Focuskeyword = "focus",
                Url = "http://localhost/empty/"
            };

            var analyzer = new KeywordUrlAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.IsTrue(!result.ResultRules.First().Tokens.Any());
            Assert.AreEqual("url_doesnt_contain_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithRootNode_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            var pageData = new PageData()
            {
                Focuskeyword = "focus",
                Url = "http://localhost/"
            };

            var analyzer = new KeywordUrlAnalyzer();
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("root_node", result.ResultRules.First().Alias);
        }
    }
}
