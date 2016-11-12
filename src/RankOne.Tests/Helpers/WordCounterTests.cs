using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class WordCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSimpleWordCallWithNullReturnsException()
        {
            var wordCounter = new WordCounter();

            wordCounter.GetSimpleWord(null);
        }

        [TestMethod]
        public void GetSimpleWordCallWithSimpleWordReturnsSimpleWord()
        {
            var wordCounter = new WordCounter();
            var simpleWord = "test";
            var result = wordCounter.GetSimpleWord(simpleWord);

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void GetSimpleWordIncludesNumericValues()
        {
            var wordCounter = new WordCounter();
            var wordWithNumbers = "test123";
            var result = wordCounter.GetSimpleWord(wordWithNumbers);

            Assert.AreEqual("test123", result);
        }

        [TestMethod]
        public void GetSimpleWordReplacesSpaces()
        {
            var wordCounter = new WordCounter();
            var twoWords = "test test";
            var result = wordCounter.GetSimpleWord(twoWords);

            Assert.AreEqual("testtest", result);
        }

        [TestMethod]
        public void GetSimpleWordCallFiltersSpecialCharacters()
        {
            var wordCounter = new WordCounter();
            var specialCharacters = "test!@#$!@";
            var result = wordCounter.GetSimpleWord(specialCharacters);

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void GetSimpleWordCallLeavesHyphens()
        {
            var wordCounter = new WordCounter();
            var hyphenedWord = "test-test";
            var result = wordCounter.GetSimpleWord(hyphenedWord);

            Assert.AreEqual("test-test", result);
        }

        [TestMethod]
        public void GetSimpleWordReplacesEncodedHtmlTags()
        {
            var wordCounter = new WordCounter();
            var specialCharacters = "&lt;span&gt;test&lt;/span&gt;";
            var result = wordCounter.GetSimpleWord(specialCharacters);

            Assert.AreEqual("test", result);
        }
    }
}
