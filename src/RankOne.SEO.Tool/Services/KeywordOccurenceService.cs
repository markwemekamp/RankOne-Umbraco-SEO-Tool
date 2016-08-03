using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using RankOne.Models;

namespace RankOne.Services
{
    public class WordOccurenceService
    {
        public IEnumerable<KeyValuePair<string, int>> GetKeywords(HtmlResult result, int numberOfWordsToReturn = 10, int minimumWordLength = 4)
        {
            var occurences = new Dictionary<string, int>();

            var rules = result.Document.SelectNodes("//text()");

            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    var xtext = rule.InnerText;

                    var ruleWords =
                        xtext.Split(new[] {'.', '?', '!', ' ', ':', ','}, StringSplitOptions.RemoveEmptyEntries)
                            .Where(x => x.Length > minimumWordLength);

                    foreach (var word in ruleWords)
                    {
                        var lowerWord = word.ToLower();
                        var rgx = new Regex("[^a-zA-Z0-9 -]");
                        lowerWord = rgx.Replace(WebUtility.HtmlDecode(lowerWord), "");
                        if (!string.IsNullOrWhiteSpace(lowerWord))
                        {
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
                }

            }
            return occurences.OrderByDescending(x => x.Value).Take(numberOfWordsToReturn);
        }
    }
}
