using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IHtmlHelper
    {
        HtmlResult GetHtmlResult(string htmlString);
    }
}