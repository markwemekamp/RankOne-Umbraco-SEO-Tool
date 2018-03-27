using HtmlAgilityPack;
using RankOne.Collections;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class WordCounter : IWordCounter
    {
        public int MinimumWordLength { get; set; }

        public WordCounter()
        {
            MinimumWordLength = 4;
        }

        public WordOccurenceCollection GetKeywords(HtmlNode htmlNode)
        {
            if (htmlNode == null) throw new ArgumentNullException(nameof(htmlNode));

            var occurences = new WordOccurenceCollection();

            var textBlocks = htmlNode.SelectNodes("//*[not(self::script) and not(self::style)]//text()");

            if (textBlocks != null)
            {
                var textBlocksWithText = textBlocks.Where(x => !string.IsNullOrWhiteSpace(x.InnerText)).Select(x => x.InnerHtml);

                foreach (var text in textBlocksWithText)
                {
                    var occurencesInBlock = CountOccurencesForText(text);
                    occurences = occurences.Merge(occurencesInBlock);
                }
            }
            return occurences;
        }

        private WordOccurenceCollection CountOccurencesForText(string textBlockText)
        {
            if (textBlockText == null) throw new ArgumentNullException(nameof(textBlockText));

            var occurences = new WordOccurenceCollection();

            // Split text to words
            var words =
                textBlockText.Split(new[] { '.', '?', '!', ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Length >= MinimumWordLength);

            foreach (var word in words)
            {
                var simpleWord = word.Simplify();
                if (!string.IsNullOrWhiteSpace(simpleWord) && simpleWord.Length >= MinimumWordLength)
                {
                    occurences.Add(simpleWord);
                }
            }

            return occurences;
        }
    }
}