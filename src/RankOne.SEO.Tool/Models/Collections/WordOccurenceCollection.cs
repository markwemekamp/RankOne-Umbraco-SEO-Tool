using System.Collections.Generic;
using System.Linq;

namespace RankOne.Models.Collections
{
    public class WordOccurenceCollection : List<WordOccurence>
    {
        public void Merge(WordOccurenceCollection mergeDictionary)
        {
            foreach (var item in mergeDictionary)
            {
                IncreaseCount(item.Word, item.OccurenceCount);
            }
        }

        public void IncreaseCount(string word, int increment = 1)
        {
            var occurence = this.FirstOrDefault(x => x.Word == word);
            if (occurence != null)
            {
                occurence.OccurenceCount += increment;
            }
            else
            {
                Add(new WordOccurence { Word = word, OccurenceCount = increment });
            }
        }

        public int GetWordCount(string word)
        {
            var occurence = this.FirstOrDefault(x => x.Word == word);
            if (occurence != null)
            {
                return occurence.OccurenceCount;
            }
            return 0;  
        }
    }
}
