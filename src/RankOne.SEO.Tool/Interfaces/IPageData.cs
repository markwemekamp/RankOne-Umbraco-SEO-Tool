using HtmlAgilityPack;

namespace RankOne.Interfaces
{
    public interface IPageData
    {
        HtmlNode Document { get; set; }
        string Focuskeyword { get; set; }
        string Url { get; set; }
    }
}