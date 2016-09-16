using RankOne.Attributes;
using RankOne.Models;
using RankOne.Services;

namespace RankOne.Summaries
{
    [Summary(SortOrder=2)]
    public class KeywordsSummary : BaseSummary
    {
        public KeywordsSummary(HtmlResult htmlResult) : base(htmlResult)
        {
            Name = "Keywords";
        }

        public override Analysis GetAnalysis()
        {
            Analysis analysis;

            if (!string.IsNullOrEmpty(FocusKeyword) && FocusKeyword != "undefined")
            {
                FocusKeyword = FocusKeyword.ToLower();

                var wordOccurenceService = new WordOccurenceService();

                var topwords = wordOccurenceService.GetKeywords(HtmlResult);

                var information = new AnalysisInformation { Alias = "keywordanalyzer_top_words" };
                information.Tokens.Add(FocusKeyword);
                foreach (var word in topwords)
                {
                    information.Tokens.Add(word.Key);
                    information.Tokens.Add(word.Value.ToString());
                }
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
    }
}