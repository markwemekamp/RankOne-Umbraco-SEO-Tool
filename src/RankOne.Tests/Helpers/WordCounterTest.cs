using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;
using System.Linq;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class WordCounterTest
    {
        private WordCounter _wordCounter;

        [TestInitialize]
        public void TestInit()
        {
            _wordCounter = new WordCounter();
        }

        [TestMethod]
        public void MinimumWordLength_OnGet_DefaultValueIs4()
        {
            Assert.AreEqual(4, _wordCounter.MinimumWordLength);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetKeywords_OnExecuteWithNull_ThrowsException()
        {
            _wordCounter.GetKeywords(null);
        }

        [TestMethod]
        public void GetKeywords_OnExecute_ReturnsOrderedListOfMostUsedWords()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html>test1 test1 test1 test1 test2 test2 test3 test4 test5 test6 test1 test2 test3 test3 test3 test7 test8 test9 test10 test11 test6</html>");

            var result = _wordCounter.GetKeywords(doc.DocumentNode);

            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.Count());
            Assert.AreEqual(5, result.FirstOrDefault(x => x.Key == "test1").Value);
            Assert.AreEqual(3, result.FirstOrDefault(x => x.Key == "test2").Value);
            Assert.AreEqual(4, result.FirstOrDefault(x => x.Key == "test3").Value);
            Assert.AreEqual(1, result.FirstOrDefault(x => x.Key == "test4").Value);
            Assert.AreEqual(1, result.FirstOrDefault(x => x.Key == "test5").Value);
            Assert.AreEqual(2, result.FirstOrDefault(x => x.Key == "test6").Value);
        }
    }
}