using System;
using RankOne.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RankOne.Tests.Collections
{
    [TestClass]
    public class WordOccurenceCollectionTest
    {
        [TestMethod]
        public void GetWordCount_OnExecute_ReturnsTheOccurenceOfTheWords()
        {
            var wordOccurendeCollection = new WordOccurenceCollection
            {
                "word",
                "word",
                "word",
                "word",
                "test",
                "test",
                "test"
            };

            var resultForWord = wordOccurendeCollection.GetWordCount("word");
            var resultForTest = wordOccurendeCollection.GetWordCount("test");

            Assert.AreEqual(4, resultForWord);
            Assert.AreEqual(3, resultForTest);
        }

        [TestMethod]
        public void Merge_OnExecute_ReturnsNewMergedCollection()
        {
            var wordOccurendeCollection = new WordOccurenceCollection {"word", "word"};
            var wordOccurendeCollection2 = new WordOccurenceCollection {"word", "word"};


            var mergedCollection = wordOccurendeCollection.Merge(wordOccurendeCollection2);

            Assert.AreNotEqual(wordOccurendeCollection, mergedCollection);
            Assert.AreNotEqual(wordOccurendeCollection2, mergedCollection);
            Assert.AreEqual(4, mergedCollection.GetWordCount("word"));
        }

        [TestMethod]
        public void GetWordCount_OnExecuteWithWordNotInCollection_ReturnsZero()
        {
            var wordOccurendeCollection = new WordOccurenceCollection { "word" };

            var result = wordOccurendeCollection.GetWordCount("test");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetWordCount_OnExecuteWithNull_ThrowsException()
        {
            var wordOccurendeCollection = new WordOccurenceCollection { "word" };

            wordOccurendeCollection.GetWordCount(null);
        }
    }
}
