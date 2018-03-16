using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IScoreService
    {
        PageScore GetScore(PageAnalysis pageAnalysis);
    }
}