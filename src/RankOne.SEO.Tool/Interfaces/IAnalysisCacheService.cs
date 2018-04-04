using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IAnalysisCacheService
    {
        void Save(int id, PageAnalysis pageAnalysis);
    }
}