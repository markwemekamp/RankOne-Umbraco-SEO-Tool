using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IAnalysisCacheRepository
    {
        void Save(int id, PageAnalysis pageAnalysis);
    }
}