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
    public class KeywordTitleAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new KeywordTitleAnalyzer((IHtmlTagHelper)null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordTitleAnalyzer(new HtmlTagHelper(), (IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><title>focus</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("title_contains_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordPresentButNotInFront_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><title>test test test focus</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("title_not_in_front", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithKeywordNotPresent_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><title>empty</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.IsTrue(!result.ResultRules.First().Tokens.Any());
            Assert.AreEqual("title_doesnt_contain_keyword", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoTitleTag_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div>focus</div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_title_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleTitleTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<title><div><title>focus</title></div></title>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_title_tags", result.ResultRules.First().Alias);
        }
    }
}