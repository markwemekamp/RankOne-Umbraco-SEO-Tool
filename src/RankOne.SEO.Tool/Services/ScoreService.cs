using System.Linq;
using RankOne.Models;

namespace RankOne.Services
{
    public class ScoreService
    {
        public PageScore GetScore(PageAnalysis pageAnalysis)
        {
            var pageScore = new PageScore();
            var totalScore = 0;
            foreach (var analyzerResult in pageAnalysis.AnalyzerResults)
            {
                var analysis = analyzerResult.Analysis;

                foreach (var result in analysis.Results)
                {
                    pageScore.ErrorCount += result.ErrorCount;
                    pageScore.WarningCount += result.WarningCount;
                    pageScore.HintCount += result.HintCount;
                    pageScore.SuccessCount += result.SuccessCount;
                    result.Score = GetResultScore(result);
                    totalScore += result.Score;
                }
            }
            pageScore.OverallScore = totalScore/pageAnalysis.AnalyzerResults.Sum(x => x.Analysis.Results.Count);

            return pageScore;
        }

        public int GetResultScore(AnalyzeResult result)
        {
            var score = 100;
            if (result.ErrorCount > 0)
            {
                score = 0;
            }
            score -= result.WarningCount * 50;
            score -= result.HintCount * 25;
            if (score < 0)
            {
                score = 0;
            }
            return score;
        }
    }
}
