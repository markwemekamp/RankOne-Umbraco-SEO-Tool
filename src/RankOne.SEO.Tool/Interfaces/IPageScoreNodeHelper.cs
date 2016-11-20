using System.Collections.Generic;
using RankOne.Models;
using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IPageScoreNodeHelper
    {
        List<PageScoreNode> GetPageHierarchy(IEnumerable<IPublishedContent> nodeCollection, bool useCache);
    }
}
