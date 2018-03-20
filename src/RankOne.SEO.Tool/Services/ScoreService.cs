using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Services
{
    public class ScoreService : IScoreService
    {
        public PageScore GetScore(PageAnalysis pageAnalysis)
        {
            if (pageAnalysis == null) throw new ArgumentNullException(nameof(pageAnalysis));

            var analyzers = pageAnalysis.SummaryResults.SelectMany(x => x.Analysis.Results);

            return CalculatePageScore(analyzers);
        }

        private PageScore CalculatePageScore(IEnumerable<AnalyzeResult> analyzers)
        {
            var pageScore = new PageScore
            {
                ErrorCount = analyzers.Sum(x => x.ErrorCount),
                WarningCount = analyzers.Sum(x => x.WarningCount),
                HintCount = analyzers.Sum(x => x.HintCount),
                SuccessCount = analyzers.Sum(x => x.SuccessCount)
            };
            var totalScore = analyzers.Sum(CalculateResultScore);
            var totalWeight = analyzers.Sum(x => x.Weight);

            pageScore.OverallScore = CalculateOverallScore(totalScore, totalWeight);

            return pageScore;
        }

        private int CalculateOverallScore(double totalScore, double totalWeight)
        {
            return (int)Math.Round(totalScore / (totalWeight / 100.0));
        }

        /// <summary>
        /// Gets the result score per analyzer.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Score for the analyzer</returns>
        private int CalculateResultScore(AnalyzeResult result)
        {
            var score = 100;

            // If there are any errors, the score is 0
            if (result.ErrorCount > 0) return 0;

            // Each warning costs 50%
            score -= result.WarningCount * 50;

            // Each hint costs 25 %
            score -= result.HintCount * 25;
            if (score < 0) return 0;
            return score;
        }
    }
}