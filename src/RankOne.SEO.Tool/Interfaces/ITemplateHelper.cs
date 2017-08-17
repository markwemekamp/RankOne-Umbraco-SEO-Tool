using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface ITemplateHelper
    {
        string GetNodeHtml(IPublishedContent node);
    }
}