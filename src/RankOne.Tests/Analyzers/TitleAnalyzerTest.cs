using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class TitleAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new TitleAnalyzer(null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new TitleAnalyzer(new HtmlTagHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Options_OnGets_ReturnDefaultValues()
        {
            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());

            Assert.AreEqual(60, analyzer.MaximumLength);
            Assert.AreEqual(5, analyzer.MinimumLength);
        }

        [TestMethod]
        public void Options_OnGetWithOverridenValues_ReturnOverridenValues()
        {
            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "MaximumLength", Value = "1"},
                    new Option(){ Key = "MinimumLength", Value = "2"}
                }
            };

            Assert.AreEqual(1, analyzer.MaximumLength);
            Assert.AreEqual(2, analyzer.MinimumLength);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoHeadTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_head_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleHeadTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><head></head></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_head_tags", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoTitleTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
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
            doc.LoadHtml("<head><title></title><title></title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_title_tags", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithEmptyTitle_SetsAnalyzeResult()
        {
            var title = Utils.GenerateString(61);

            var doc = new HtmlDocument();
            doc.LoadHtml($"<head><title></title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_title_value", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithShortTitle_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<head><title>test</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("title_too_short", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithLongTitle_SetsAnalyzeResult()
        {
            var title = Utils.GenerateString(61);

            var doc = new HtmlDocument();
            doc.LoadHtml($"<head><title>{title}</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("title_too_long", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithTitleOf50Characters_SetsAnalyzeResult()
        {
            var title = Utils.GenerateString(60);

            var doc = new HtmlDocument();
            doc.LoadHtml($"<head><title>{title}</title></head>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("title_success", result.ResultRules.First().Alias);
        }
    }
}