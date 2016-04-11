using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Services
{
    public class KeywordOccurenceService
    {
        public IEnumerable<KeyValuePair<string, int>> GetKeywords(HtmlResult result, int numberOfWordsToReturn = 10, int minimumWordLength = 4)
        {
            var occurences = new Dictionary<string, int>();

            var text = result.Document.Descendants();

            foreach (var rule in text)
            {
                var xtext = rule.InnerText;

                var ruleWords = xtext.Split(new [] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > minimumWordLength);

                foreach (var word in ruleWords)
                {
                    var lowerWord = word.ToLower().Trim();
                    if (occurences.ContainsKey(lowerWord))
                    {
                        occurences[lowerWord]++;
                    }
                    else
                    {
                        occurences.Add(lowerWord, 1);
                    }
                }
            }

            return occurences.OrderByDescending(x => x.Value).Take(numberOfWordsToReturn);
        }
    }
}
