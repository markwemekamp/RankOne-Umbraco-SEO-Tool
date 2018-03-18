using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IDashboardDataService
    {
        void Initialize();

        IEnumerable<PageScoreNode> GetHierarchyFromCache();

        IEnumerable<PageScoreNode> GetUpdatedHierarchy();
    }
}