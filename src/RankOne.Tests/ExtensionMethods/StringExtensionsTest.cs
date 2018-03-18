using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.ExtensionMethods;

namespace RankOne.Tests.ExtensionMethods
{
    [TestClass]
    public class StringExtensionsTest
    {
        #region UrlFriendly

        public void UrlFriendly_OnExecuteWithSimpleWord_ReturnsSimpleWord()
        {
            var simpleWord = "test";
            var result = simpleWord.UrlFriendly();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void UrlFriendly_OnExecuteWithNumbers_IncludesNumbers()
        {
            var wordWithNumbers = "test123";
            var result = wordWithNumbers.UrlFriendly();

            Assert.AreEqual("test123", result);
        }

        [TestMethod]
        public void UrlFriendly_OnExecute_ReplacesSpaces()
        {
            var twoWords = "test test";
            var result = twoWords.UrlFriendly();

            Assert.AreEqual("test-test", result);
        }

        [TestMethod]
        public void UrlFriendly_OnExecute_FiltersSpecialCharacters()
        {
            var specialCharacters = "test!@#$!@";
            var result = specialCharacters.UrlFriendly();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void UrlFriendly_OnExecute_LeavesHyphens()
        {
            var hyphenedWord = "test-test";
            var result = hyphenedWord.UrlFriendly();

            Assert.AreEqual("test-test", result);
        }

        [TestMethod]
        public void UrlFriendly_OnExecute_ReplacesEncodedHtmlTags()
        {
            var specialCharacters = "&lt;span&gt;test&lt;/span&gt;";
            var result = specialCharacters.UrlFriendly();

            Assert.AreEqual("test", result);
        }

        #endregion UrlFriendly



        #region Simplify

        [TestMethod]
        public void Simplify_OnExecuteWithSimpleWord_ReturnsSimpleWord()
        {
            var simpleWord = "test";
            var result = simpleWord.Simplify();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void Simplify_OnExecuteWithNumbers_IncludesNumbers()
        {
            var wordWithNumbers = "test123";
            var result = wordWithNumbers.Simplify();

            Assert.AreEqual("test123", result);
        }

        [TestMethod]
        public void Simplify_OnExecute_ReplacesSpaces()
        {
            var twoWords = "test test";
            var result = twoWords.Simplify();

            Assert.AreEqual("testtest", result);
        }

        [TestMethod]
        public void Simplify_OnExecute_FiltersSpecialCharacters()
        {
            var specialCharacters = "test!@#$!@";
            var result = specialCharacters.Simplify();

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void Simplify_OnExecute_LeavesHyphens()
        {
            var hyphenedWord = "test-test";
            var result = hyphenedWord.Simplify();

            Assert.AreEqual("test-test", result);
        }

        [TestMethod]
        public void Simplify_OnExecute_ReplacesEncodedHtmlTags()
        {
            var specialCharacters = "&lt;span&gt;test&lt;/span&gt;";
            var result = specialCharacters.Simplify();

            Assert.AreEqual("test", result);
        }

        #endregion Simplify
    }
}