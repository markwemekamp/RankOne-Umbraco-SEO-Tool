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
    public class MetaDescriptionAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new MetaDescriptionAnalyzer(null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new MetaDescriptionAnalyzer(new HtmlTagHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Options_OnGets_ReturnDefaultValues()
        {
            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());

            Assert.AreEqual(150, analyzer.MaximumLength);
            Assert.AreEqual(20, analyzer.MinimumLength);
            Assert.AreEqual(50, analyzer.AcceptableLength);
        }

        [TestMethod]
        public void Options_OnGetWithOverridenValues_ReturnOverridenValues()
        {
            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "MaximumLength", Value = "1"},
                    new Option(){ Key = "MinimumLength", Value = "2"},
                    new Option(){ Key = "AcceptableLength", Value = "3"}
                }
            };

            Assert.AreEqual(1, analyzer.MaximumLength);
            Assert.AreEqual(2, analyzer.MinimumLength);
            Assert.AreEqual(3, analyzer.AcceptableLength);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div>focus</div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_meta_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaDescriptionTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"keyword\" content=\"test\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_meta_description_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleMetaDescriptionTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"description\" content=\"test\" /><meta name=\"description\" content=\"test\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_meta_description_tags", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionTagButNoValue_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"description\" content=\"\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_description_value", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueLongerThan150Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 151; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("description_too_long", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueShorterThan20Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 19; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("description_too_short", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueWith20Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 49; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("description_shorter_then_acceptable", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueWith49Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 49; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("description_shorter_then_acceptable", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueWith50Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 50; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("description_perfect", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaDescriptionValueWith150Charachters_SetsAnalyzeResult()
        {
            var description = "";

            for (int i = 0; i < 150; i++) { description += "t"; }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"description\" content=\"{description}\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("description_perfect", result.ResultRules.First().Alias);
        }
    }
}