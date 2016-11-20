using RankOne.Models;
using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IAnalyzeService
    {
        PageAnalysis CreateAnalysis(IPublishedContent node, string focusKeyword = null);
    }
}
