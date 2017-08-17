using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IFocusKeywordHelper
    {
        string GetFocusKeyword(IPublishedContent node);
    }
}