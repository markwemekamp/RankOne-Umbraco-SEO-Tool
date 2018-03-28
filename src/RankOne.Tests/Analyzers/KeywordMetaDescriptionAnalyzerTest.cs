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
    public class KeywordMetaDescriptionAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordMetaDescriptionAnalyzer((IHtmlTagHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><meta name=\"description\" content=\"focus\" /></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("meta_description_contains_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordNotPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><meta name=\"description\" content=\"empty\" /></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.IsTrue(!result.ResultRules.First().Tokens.Any());
            Assert.AreEqual("meta_description_doesnt_contain_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaTag_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("no_meta_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaDescriptionTag_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><meta name=\"keyword\" content=\"focus\" /></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("no_meta_description_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleMetaDescriptionTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><meta name=\"description\" content=\"focus\" /><meta name=\"description\" content=\"focus\" /></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_meta_description_tags", result.ResultRules.First().Alias);
        }
    }
}