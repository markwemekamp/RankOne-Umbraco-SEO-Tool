using System.Linq;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class KeywordAnalyzer
    {
        private readonly HtmlResult _htmlResult;

        public KeywordAnalyzer(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var keywordOccurenceService = new KeywordOccurenceService();

            var keywords = keywordOccurenceService.GetKeywords(_htmlResult.Document);

            var information = new AnalysisInformation {Code = "keywordanalyzer_top_words"};
            foreach (var word in keywords)
            {
                information.Tokens.Add(word.Key);
                information.Tokens.Add(word.Value.ToString());
            }

            analysis.Information.Add(information);

            if (keywords.Any())
            {
                var keywordTitleAnalyzer = new KeywordTitleAnalyzer();
                analysis.Results.Add(keywordTitleAnalyzer.Analyse(_htmlResult.Document, keywords.FirstOrDefault().Key));

                var keywordMetaDescriptionAnalyzer = new KeywordMetaDescriptionAnalyzer();
                analysis.Results.Add(keywordMetaDescriptionAnalyzer.Analyse(_htmlResult.Document, keywords.FirstOrDefault().Key));
            }
            

            return analysis;
        }
    }
}