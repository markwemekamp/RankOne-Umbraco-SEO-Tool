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

        public string FocusKeyword { get; set; }

        public KeywordSummary(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            if (!string.IsNullOrEmpty(FocusKeyword) && FocusKeyword != "undefined")
            {
                var wordOccurenceService = new WordOccurenceService();

                var topwords = wordOccurenceService.GetKeywords(_htmlResult);

                var information = new AnalysisInformation { Code = "keywordanalyzer_top_words" };
                information.Tokens.Add(FocusKeyword);
                foreach (var word in topwords)
                {
                    information.Tokens.Add(word.Key);
                    information.Tokens.Add(word.Value.ToString());
                }

                analysis.Information.Add(information);


                var keywordTitleAnalyzer = new KeywordTitleAnalyzer();
                analysis.Results.Add(keywordTitleAnalyzer.Analyse(_htmlResult.Document, FocusKeyword));

                var keywordMetaDescriptionAnalyzer = new KeywordMetaDescriptionAnalyzer();
                analysis.Results.Add(keywordMetaDescriptionAnalyzer.Analyse(_htmlResult.Document, FocusKeyword));

                var keywordUrlAnalyzer = new KeywordUrlAnalyzer();
                analysis.Results.Add(keywordUrlAnalyzer.Analyse(_htmlResult.Document, FocusKeyword, _htmlResult.Url));

                var keywordContentAnalyzer = new KeywordContentAnalyzer();
                analysis.Results.Add(keywordContentAnalyzer.Analyse(_htmlResult.Document, FocusKeyword));

            }
            else
            {
                var information = new AnalysisInformation { Code = "keywordanalyzer_focus_keyword_not_set" };
                analysis.Information.Add(information);
            }

            return analysis;
        }
    }
}