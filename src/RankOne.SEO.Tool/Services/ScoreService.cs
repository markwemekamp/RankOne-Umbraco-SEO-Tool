using System.Linq;
using RankOne.Models;

namespace RankOne.Services
{
    public class ScoreService
    {
        /// <summary>
        /// Gets the score for a category.
        /// </summary>
        /// <param name="pageAnalysis">The page analysis.</param>
        /// <returns></returns>
        public PageScore GetScore(PageAnalysis pageAnalysis)
        {
            var pageScore = new PageScore();
            var totalScore = 0;
            foreach (var analyzerResult in pageAnalysis.AnalyzerResults)
            {
                var analysis = analyzerResult.Analysis;
                totalScore = CalculateScore(analysis, pageScore, totalScore);
            }

            var numberOfAnalyzers = pageAnalysis.AnalyzerResults.Sum(x => x.Analysis.Results.Count);
            pageScore.OverallScore = totalScore / numberOfAnalyzers;

            return pageScore;
        }

        private int CalculateScore(Analysis analysis, PageScore pageScore, int totalScore)
        {
            foreach (var result in analysis.Results)
            {
                pageScore.ErrorCount += result.ErrorCount;
                pageScore.WarningCount += result.WarningCount;
                pageScore.HintCount += result.HintCount;
                pageScore.SuccessCount += result.SuccessCount;
                result.Score = GetResultScore(result);
                totalScore += result.Score;
            }
            return totalScore;
        }

        /// <summary>
        /// Gets the result score per analyzer.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Score for the analyzer</returns>
        public int GetResultScore(AnalyzeResult result)
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
