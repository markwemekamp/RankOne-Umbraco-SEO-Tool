using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using RankOne.Models;

namespace RankOne.Helpers
{
    public class WordCounter
    {
        public int MinimumWordLength { get; set; }

        public WordCounter()
        {
            MinimumWordLength = 4;
        }

        public IEnumerable<KeyValuePair<string, int>> GetKeywords(HtmlResult result, int numberOfWordsToReturn = 10)
        {
            var occurences = new Dictionary<string, int>();

            var textBlocks = result.Document.SelectNodes("//text()");

            if (textBlocks != null)
            {
                foreach (var textBlock in textBlocks)
                {
                    var textBlockText = textBlock.InnerText;

                    CountOccurencesForTextBlock(textBlockText, occurences);
                }
            }
            return occurences.OrderByDescending(x => x.Value).Take(numberOfWordsToReturn);
        }

        public void CountOccurencesForTextBlock(string textBlockText, Dictionary<string, int> occurences)
        {
            var words =
                textBlockText.Split(new[] {'.', '?', '!', ' ', ':', ','}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Length > MinimumWordLength);

            foreach (var word in words)
            {
                var formattedWord = GetSimpleWord(word);
                CountWord(occurences, formattedWord);
            }
        }

        public string GetSimpleWord(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException("word", "Word parameter cannot be null");
            }

            word = word.ToLower();
            word = WebUtility.HtmlDecode(word);
            
            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            word = htmlRegex.Replace(word, string.Empty);

            var rgx = new Regex("[^a-zA-Z0-9-]");
            word = rgx.Replace(word, string.Empty);

            return word;
        }

        private void CountWord(IDictionary<string, int> occurences, string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                if (occurences.ContainsKey(word))
                {
                    occurences[word]++;
                }
                else
                {
                    occurences.Add(word, 1);
                }
            }
        }
    }
}
