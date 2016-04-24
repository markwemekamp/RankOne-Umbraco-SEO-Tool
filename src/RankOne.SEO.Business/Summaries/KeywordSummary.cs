using System.Linq;
using RankOne.Business.Analyzers;
using RankOne.Business.Analyzers.Keywords;
using RankOne.Business.Models;
using RankOne.Business.Services;

namespace RankOne.Business.Summaries
{
    public class KeywordSummary
    {
        private readonly HtmlResult _htmlResult;

        public KeywordSummary(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var keywordOccurenceService = new KeywordOccurenceService();

            var keywords = keywordOccurenceService.GetKeywords(_htmlResult);

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