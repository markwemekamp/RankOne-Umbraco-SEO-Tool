using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.ExtensionMethods;

namespace RankOne.Tests.ExtensionMethods
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void GetSimpleWordCallWithSimpleWordReturnsSimpleWord()
        {
            var simpleWord = "test";
            var result = simpleWord.ConvertToSimpleWord();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void GetSimpleWordIncludesNumericValues()
        {
            var wordWithNumbers = "test123";
            var result = wordWithNumbers.ConvertToSimpleWord();

            Assert.AreEqual("test123", result);
        }

        [TestMethod]
        public void GetSimpleWordReplacesSpaces()
        {
            var twoWords = "test test";
            var result = twoWords.ConvertToSimpleWord();

            Assert.AreEqual("testtest", result);
        }

        [TestMethod]
        public void GetSimpleWordCallFiltersSpecialCharacters()
        {
            var specialCharacters = "test!@#$!@";
            var result = specialCharacters.ConvertToSimpleWord();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void GetSimpleWordCallLeavesHyphens()
        {
            var hyphenedWord = "test-test";
            var result = hyphenedWord.ConvertToSimpleWord();

            Assert.AreEqual("test-test", result);
        }

        [TestMethod]
        public void GetSimpleWordReplacesEncodedHtmlTags()
        {
            var specialCharacters = "&lt;span&gt;test&lt;/span&gt;";
            var result = specialCharacters.ConvertToSimpleWord();

            Assert.AreEqual("test", result);
        }


    }
}
