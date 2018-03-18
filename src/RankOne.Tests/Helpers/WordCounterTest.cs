using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System.Linq;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class WordCounterTest
    {
        [TestMethod]
        public void CountOccurencesForText_OnExecute_ReturnsOccurenceOfWordInText()
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
        public void GetWordCount_OnExecuteWithNotOccuringWord_ReturnsZero()
        {
            var wordCounter = new WordCounter();

            var text = "test1 test2 test3 test4 test5 test6";

            var result = wordCounter.CountOccurencesForText(text);

            Assert.AreEqual(0, result.GetWordCount("test7"));
        }

        [TestMethod]
        public void CountOccurencesForText_OnExecuteWithShortWords_SkipsWordsThatAre3CharactersOrShorter()
        {
            var wordCounter = new WordCounter();

            var text = "test test test tes tes tes tes";

            var result = wordCounter.CountOccurencesForText(text);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result.GetWordCount("test"));
        }

        [TestMethod]
        public void CountOccurencesForText_OnExecuteWithMinimumWordLengthSetTo6_SkipsWordsThatAre5CharactersOrShorter()
        {
            var wordCounter = new WordCounter
            {
                MinimumWordLength = 6
            };

            var text = "test1234 test123 test12 test1 test";

            var result = wordCounter.CountOccurencesForText(text);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result.GetWordCount("test1234"));
            Assert.AreEqual(1, result.GetWordCount("test123"));
            Assert.AreEqual(1, result.GetWordCount("test12"));
            Assert.AreEqual(0, result.GetWordCount("test1"));
            Assert.AreEqual(0, result.GetWordCount("test"));
        }

        [TestMethod]
        public void GetKeywords_OnExecute_ReturnsOccurencesOfWords()
        {
            var wordCounter = new WordCounter();

            var text = "test1 test2 test3 test4 test5 test6 test1 test2 test3";

            var result = wordCounter.GetKeywords(text);

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count());
            Assert.AreEqual(2, result.FirstOrDefault(x => x.Key == "test1").Value);
            Assert.AreEqual(2, result.FirstOrDefault(x => x.Key == "test2").Value);
            Assert.AreEqual(2, result.FirstOrDefault(x => x.Key == "test3").Value);
            Assert.AreEqual(1, result.FirstOrDefault(x => x.Key == "test4").Value);
            Assert.AreEqual(1, result.FirstOrDefault(x => x.Key == "test5").Value);
            Assert.AreEqual(1, result.FirstOrDefault(x => x.Key == "test6").Value);
        }
    }
}