using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class ImageTagAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new ImageTagAnalyzer();
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoImageTags_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("alt_and_title_attributes_present", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithCorrectImageTag_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><img src=\"test\" alt=\"test\" title=\"test\" /></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("alt_and_title_attributes_present", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMissingAltAttribute_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><img src=\"test\" title=\"test\" /></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("missing_alt_attribute", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMissingTitleAttribute_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><img src=\"test\" alt=\"test\" /></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("missing_title_attribute", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMissingAltAndTitleAttribute_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><img src=\"test\" /></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 2);
            Assert.AreEqual(ResultType.Hint, result.ResultRules[0].Type);
            Assert.AreEqual("missing_alt_attribute", result.ResultRules[0].Alias);
            Assert.AreEqual(ResultType.Hint, result.ResultRules[1].Type);
            Assert.AreEqual("missing_title_attribute", result.ResultRules[1].Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMultipleImagesAndOneMissingAltAndTitleAttribute_SetsResult()
        {
            var analyzer = new ImageTagAnalyzer();

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><img src=\"test\" alt=\"test\" title=\"test\" /><img src=\"test\" /></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 2);
            Assert.AreEqual(ResultType.Hint, result.ResultRules[0].Type);
            Assert.AreEqual("missing_alt_attribute", result.ResultRules[0].Alias);
            Assert.AreEqual("1", result.ResultRules[0].Tokens[0]);
            Assert.AreEqual(ResultType.Hint, result.ResultRules[1].Type);
            Assert.AreEqual("missing_title_attribute", result.ResultRules[1].Alias);
            Assert.AreEqual("1", result.ResultRules[1].Tokens[0]);
        }
    }
}