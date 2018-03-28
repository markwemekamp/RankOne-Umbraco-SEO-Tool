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
    public class DeprecatedTagAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new DeprecatedTagAnalyzer((IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Options_OnGets_ReturnDefaultValues()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());

            Assert.AreEqual(12, analyzer.DeprecatedTags.Count());
            Assert.IsTrue(analyzer.DeprecatedTags.Contains("center"));
        }

        [TestMethod]
        public void Options_OnGetWithOverridenValues_ReturnOverridenValues()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "DeprecatedTags", Value = "div"}
                }
            };

            Assert.IsTrue(analyzer.DeprecatedTags.Contains("div"));
            Assert.AreEqual(1, analyzer.DeprecatedTags.Count());
        }

        [TestMethod]
        public void Analyse_OnExecuteWithDeprecatedTag_SetsResult()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());

            var doc = new HtmlDocument();
            doc.LoadHtml("<body><center>focus</center></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("tag_found", result.ResultRules.First().Alias);
            Assert.AreEqual("center", result.ResultRules.First().Tokens.First());
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoDeprecatedTag_SetsResult()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());

            var doc = new HtmlDocument();
            doc.LoadHtml("<body><div>focus</div></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("no_deprecated_tags_found", result.ResultRules.First().Alias);
        }
    }
}