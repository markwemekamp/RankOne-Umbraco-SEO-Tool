using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Summaries;
using System;
using System.Linq;

namespace RankOne.Tests.Summaries
{
    [TestClass]
    public class KeywordsSummaryTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordsSummary((IWordCounter)null);
        }

        [TestMethod]
        public void Constructor_OnExecute_SetsName()
        {
            var summary = new KeywordsSummary(new WordCounter());

            Assert.AreEqual("Keywords", summary.Name);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithFocusKeywordSetToNull_ReturnsAnalysis()
        {
            var summary = new KeywordsSummary(new WordCounter());
            summary.FocusKeyword = null;
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Information.Any());
            Assert.AreEqual("keywordanalyzer_focus_keyword_not_set", result.Information.FirstOrDefault().Alias);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithEmptyFocusKeyword_ReturnsAnalysis()
        {
            var summary = new KeywordsSummary(new WordCounter());
            summary.FocusKeyword = "";
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Information.Any());
            Assert.AreEqual("keywordanalyzer_focus_keyword_not_set", result.Information.FirstOrDefault().Alias);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithFocusKeywordSetToUndefined_ReturnsAnalysis()
        {
            var summary = new KeywordsSummary(new WordCounter());
            summary.FocusKeyword = "undefined";
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Information.Any());
            Assert.AreEqual("keywordanalyzer_focus_keyword_not_set", result.Information.FirstOrDefault().Alias);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithNoDocumentSet_ReturnsAnalysis()
        {
            var summary = new KeywordsSummary(new WordCounter());
            summary.FocusKeyword = "focus";
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Information.Any());
            Assert.AreEqual("keywordanalyzer_top_words", result.Information.FirstOrDefault().Alias);
        }

        [TestMethod]
        public void GetAnalysis_OnExecuteWithReturnsAnalysis()
        {
            var text = "";

            for (int i = 20; i > 0; i--)
            {
                for (int j = i; j > 0; j--)
                {
                    text += $"word_{i}_times ";
                }
            }

            var doc = new HtmlDocument();
            doc.LoadHtml($"<div>{text}</div>");

            var summary = new KeywordsSummary(new WordCounter());
            summary.FocusKeyword = "focus";
            summary.Document = doc.DocumentNode;
            var result = summary.GetAnalysis();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Information.Any());
            Assert.AreEqual("keywordanalyzer_top_words", result.Information.First().Alias);

            var tokens = result.Information.First().Tokens;
            Assert.AreEqual(21, tokens.Count);

            Assert.AreEqual("focus", tokens[0]);
            Assert.AreEqual("word_20_times", tokens[1]);
            Assert.AreEqual("20", tokens[2]);
            Assert.AreEqual("word_11_times", tokens[19]);
            Assert.AreEqual("11", tokens[20]);
        }
    }
}