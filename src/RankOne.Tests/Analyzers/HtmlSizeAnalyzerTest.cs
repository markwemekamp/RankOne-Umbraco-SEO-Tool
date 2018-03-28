using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class HtmlSizeAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForByteSizeHelper_ThrowArgumentNullException()
        {
            new HtmlSizeAnalyzer(new ByteSizeHelper(), (IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new HtmlSizeAnalyzer((IByteSizeHelper)null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Options_OnGets_ReturnDefaultValues()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper());

            Assert.AreEqual(33792, analyzer.MaximumSizeInBytes);
        }

        [TestMethod]
        public void Options_OnGetWithOverridenValues_ReturnOverridenValues()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "MaximumSizeInBytes", Value = "1"},
                }
            };

            Assert.AreEqual(1, analyzer.MaximumSizeInBytes);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithLargerHtmlThanMaximum_SetsResult()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "MaximumSizeInBytes", Value = "20"},
                }
            };

            var doc = new HtmlDocument();
            doc.LoadHtml("<html><head>Very large</head></html>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("html_size_too_large", result.ResultRules.First().Alias);
            Assert.AreEqual("36 bytes", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("20 bytes", result.ResultRules.First().Tokens[1]);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithSmallerHtmlThanMaximum_SetsResult()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "MaximumSizeInBytes", Value = "20"},
                }
            };

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
            Assert.AreEqual("html_size_small", result.ResultRules.First().Alias);
            Assert.AreEqual("13 bytes", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("20 bytes", result.ResultRules.First().Tokens[1]);
        }
    }
}