using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class WordCounterTests
    {
        [TestMethod]
        public void Test()
        {
            var wordCounter = new WordCounter();

            var text = "test1 test2 test3 test4 test5 test6 test1 test2";

            var result = wordCounter.CountOccurencesForText(text);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(2, result.GetWordCount("test1"));
            Assert.AreEqual(2, result.GetWordCount("test2"));
            Assert.AreEqual(1, result.GetWordCount("test3"));
            Assert.AreEqual(1, result.GetWordCount("test4"));
            Assert.AreEqual(1, result.GetWordCount("test5"));
            Assert.AreEqual(1, result.GetWordCount("test6"));
        }

        [TestMethod]
        public void WordCounterOnlyCountsWordsThatAre4CharactersOrLonger()
        {
            var wordCounter = new WordCounter();

            var text = "test test test tes tes tes tes";

            var result = wordCounter.CountOccurencesForText(text);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result.GetWordCount("test"));
        }
    }
}
