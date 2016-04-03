using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class KeywordOccurenceAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(XDocument document)
        {
            const int minimumWordLength = 4;
            const int numberOfWordsToReturn = 3;

            var result = new AnalyzeResult();
            result.Title = "keywordanalyzer_title";

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

            var topWords = occurences.OrderByDescending(x => x.Value).Take(numberOfWordsToReturn);

            /*var information = new ResultRule {Code = "keywordanalyzer_top_words", Type = ResultType.Information};
            foreach (var word in topWords)
            {
                information.Tokens.Add(word.Key);
                information.Tokens.Add(word.Value.ToString());
            }

            result.ResultRules.Add(information);*/

            return result;
        }
    }
}
