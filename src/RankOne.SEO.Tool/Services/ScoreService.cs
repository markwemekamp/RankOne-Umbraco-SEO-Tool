using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Services
{
    public class ScoreService : IScoreService
    {
        /// <summary>
        /// Gets the score for a category.
        /// </summary>
        /// <param name="pageAnalysis">The page analysis.</param>
        /// <returns></returns>
        public PageScore GetScore(PageAnalysis pageAnalysis)
        {
            var pageScore = new PageScore();
            var totalScore = 0.0;
            var totalWeight = 0.0;
            // loop through summaries
            foreach (var summaryResult in pageAnalysis.SummaryResults)
            {
                // loop through analyzers
                foreach (var result in summaryResult.Analysis.Results)
                {
                    pageScore.ErrorCount += result.ErrorCount;
                    pageScore.WarningCount += result.WarningCount;
                    pageScore.HintCount += result.HintCount;
                    pageScore.SuccessCount += result.SuccessCount;
                    var score = GetResultScore(result);

                    totalScore += score;
                    totalWeight += result.Weight;
                }
            }

            pageScore.OverallScore = (int)Math.Round(totalScore / (totalWeight / 100.0));

            return pageScore;
        }

        /// <summary>
        /// Gets the result score per analyzer.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Score for the analyzer</returns>
        private int GetResultScore(AnalyzeResult result)
        {
            var score = 100;
            // If there are any errors, the score is 0
            if (result.ErrorCount > 0)
            {
                score = 0;
            }

            // Each warning costs 50%
            score -= result.WarningCount * 50;

            // Each hint costs 25 %
            score -= result.HintCount * 25;
            if (score < 0)
            {
                score = 0;
            }
            return score;
        }
    }
}