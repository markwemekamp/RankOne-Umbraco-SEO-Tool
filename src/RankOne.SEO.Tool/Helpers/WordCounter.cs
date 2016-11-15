using System;
using System.Collections.Generic;
using System.Linq;
using RankOne.ExtensionMethods;
using RankOne.Models;
using RankOne.Models.Collections;

namespace RankOne.Helpers
{
    public class WordCounter
    {
        public int MinimumWordLength { get; set; }

        public WordCounter()
        {
            MinimumWordLength = 4;
        }

        public IEnumerable<WordOccurence> GetKeywords(string text)
        {
            return CountOccurencesForText(text).OrderByDescending(x => x.OccurenceCount);
        }
        public IEnumerable<WordOccurence> GetKeywords(HtmlResult html)
        {
            var occurences = new WordOccurenceCollection();

            var textBlocks = html.Document.SelectNodes("//text()");

            if (textBlocks != null)
            {
                foreach (var textBlock in textBlocks)
                {
                    var textBlockText = textBlock.InnerText;
                    var occurencesInBlock = CountOccurencesForText(textBlockText);
                    occurences.Merge(occurencesInBlock);
                }
            }
            return occurences.OrderByDescending(x => x.OccurenceCount);
        }

        public WordOccurenceCollection CountOccurencesForText(string textBlockText)
        {
            var occurences = new WordOccurenceCollection();

            // Split text to words
            var words =
                textBlockText.Split(new[] {'.', '?', '!', ' ', ':', ','}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Length >= MinimumWordLength);

            foreach (var word in words)
            {
                var simpleWord = word.Simplify();
                occurences.IncreaseCount(simpleWord);
            }

            return occurences;
        }
    }
}
