using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RankOne.Business.Analyzers
{
    public class KeywordOccurenceService
    {
        public IEnumerable<KeyValuePair<string, int>> GetKeywords(XDocument document, int numberOfWordsToReturn = 10, int minimumWordLength = 4)
        {
            var occurences = new Dictionary<string, int>();

            var text = document.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Text);

            foreach (var rule in text)
            {
                var xtext = (XText) rule;

                var ruleWords = xtext.Value.Split(new [] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > minimumWordLength);

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
