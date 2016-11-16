using System.Linq;
using RankOne.Attributes;
using RankOne.Helpers;
using RankOne.Models;
using RankOne.Services;

namespace RankOne.Summaries
{
    [Summary(Alias = "keywordanalyzer", SortOrder = 2)]
    public class KeywordsSummary : BaseSummary
    {
        private readonly WordCounter _wordOccurenceHelper;

        public KeywordsSummary() : this(new WordCounter())
        { }

        public KeywordsSummary(WordCounter wordOccurenceService)
        {
            _wordOccurenceHelper = wordOccurenceService;
            Name = "Keywords";
        }

        public override Analysis GetAnalysis()
        {
            Analysis analysis;

            if (!string.IsNullOrEmpty(FocusKeyword) && FocusKeyword != "undefined")
            {
                var focusKeyword = FocusKeyword.ToLower();

                var information = GetAnalysisInformation(focusKeyword);
                analysis = base.GetAnalysis();
                analysis.Information.Add(information);
            }
            else
            {
                analysis = new Analysis();
                var information = new AnalysisInformation { Alias = "keywordanalyzer_focus_keyword_not_set" };
                analysis.Information.Add(information);
            }

            return analysis;
        }

        private AnalysisInformation GetAnalysisInformation(string focusKeyword)
        {
            var topwords = _wordOccurenceHelper.GetKeywords(HtmlResult).Take(10);

            var information = new AnalysisInformation {Alias = "keywordanalyzer_top_words"};
            information.Tokens.Add(focusKeyword);
            foreach (var wordOccurence in topwords)
            {
                information.Tokens.Add(wordOccurence.Key);
                information.Tokens.Add(wordOccurence.Value.ToString());
            }
            return information;
        }
    }
}