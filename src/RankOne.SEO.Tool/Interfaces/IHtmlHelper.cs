using RankOne.Models;
using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IHtmlHelper
    {
        string GetTemplateHtml(IPublishedContent node);

        HtmlResult GetHtmlResult(string htmlString);
    }
}