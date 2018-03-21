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
    public class AdditionalCallAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new AdditionalCallAnalyzer((IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithLessThan15ExternalCalls_SetsAnalyzeResult()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());

            var doc = new HtmlDocument();
            doc.LoadHtml("<body><img src=\"/\" /></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("30", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("15", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("less_than_acceptable_calls", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithLessThan15ExternalCalls_TokensGetSetCorrecly()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());

            var doc = new HtmlDocument();
            doc.LoadHtml("<body><link href=\"/\" rel=\"stylesheet\"/><script src=\"/\"></script><img src=\"/\" /><object data=\"/\"></object></body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("4", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("1", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("30", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("15", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("less_than_acceptable_calls", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMoreThan15ExternalCalls_TokensGetSetCorrecly()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());

            string html = "";
            for (int i = 0; i < 16; i++)
            {
                html += "<img src=\"/\" />";
            }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<body>{html}</body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("16", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("16", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("30", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("15", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("more_than_acceptable_calls", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMoreThan30ExternalCalls_TokensGetSetCorrecly()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());

            string html = "";
            for (int i = 0; i < 31; i++)
            {
                html += "<img src=\"/\" />";
            }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<body>{html}</body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("31", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("31", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("30", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("15", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("more_than_maximum_calls", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithCustomOptionsWithMoreThan5ExternalCalls_TokensGetSetCorrecly()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());
            analyzer.Options = new List<Option> { new Option() { Key = "AcceptableAdditionalCalls", Value = "5" } };

            string html = "";
            for (int i = 0; i < 6; i++)
            {
                html += "<img src=\"/\" />";
            }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<body>{html}</body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Hint, result.ResultRules.First().Type);
            Assert.AreEqual("6", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("6", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("30", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("5", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("more_than_acceptable_calls", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithCustomOptionsWithMoreThan10ExternalCalls_TokensGetSetCorrecly()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());
            analyzer.Options = new List<Option> { new Option() { Key = "MaximumAdditionalCalls", Value = "10" } };

            string html = "";
            for (int i = 0; i < 11; i++)
            {
                html += "<img src=\"/\" />";
            }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<body>{html}</body>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("11", result.ResultRules.First().Tokens[0]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[1]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[2]);
            Assert.AreEqual("11", result.ResultRules.First().Tokens[3]);
            Assert.AreEqual("0", result.ResultRules.First().Tokens[4]);
            Assert.AreEqual("10", result.ResultRules.First().Tokens[5]);
            Assert.AreEqual("15", result.ResultRules.First().Tokens[6]);
            Assert.AreEqual("more_than_maximum_calls", result.ResultRules.First().Alias);
        }
    }
}
