using System.Collections.Generic;

namespace RankOne.Collections
{
    public class WordOccurenceCollection : Dictionary<string, int>
    {
        public WordOccurenceCollection()
        { }

        public WordOccurenceCollection(WordOccurenceCollection collection) : base(collection)
        { }

        public WordOccurenceCollection Merge(WordOccurenceCollection mergeDictionary)
        {
            var mergedCollection = new WordOccurenceCollection(this);

            foreach (var key in mergeDictionary.Keys)
            {
                mergedCollection.Add(key, mergeDictionary[key]);
            }

            return mergedCollection;
        }

        public new void Add(string word, int increment = 1)
        {
            if (!ContainsKey(word))
            {
                base.Add(word, 1);
            }
            else
            {
                this[word] += increment;
            }
        }

        public int GetWordCount(string word)
        {
            if (ContainsKey(word))
            {
                return this[word];
            }
            else
            {
                return 0;
            }
        }
    }
}