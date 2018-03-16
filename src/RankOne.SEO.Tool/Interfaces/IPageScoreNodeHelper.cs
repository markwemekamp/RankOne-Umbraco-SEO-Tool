using RankOne.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IPageScoreNodeHelper
    {
        IEnumerable<PageScoreNode> GetPageScoresFromCache(IEnumerable<IPublishedContent> nodeCollection);
        IEnumerable<PageScoreNode> UpdatePageScores(IEnumerable<IPublishedContent> nodeCollection);
    }
}