using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;

namespace RankOne.Summaries
{
    public class KeywordsSummary : BaseSummary
    {
        private readonly IWordCounter _wordOccurenceHelper;

        public KeywordsSummary() : this(RankOneContext.Instance)
        { }

        public KeywordsSummary(IRankOneContext rankOneContext) : this(rankOneContext.WordCounter.Value)
        { }

        public KeywordsSummary(IWordCounter wordOccurenceHelper)
        {
            _wordOccurenceHelper = wordOccurenceHelper;
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

            var information = new AnalysisInformation { Alias = "keywordanalyzer_top_words" };
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