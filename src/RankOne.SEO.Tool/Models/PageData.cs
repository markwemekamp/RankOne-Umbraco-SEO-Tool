using HtmlAgilityPack;
using RankOne.Interfaces;

namespace RankOne.Models
{
    public class PageData : IPageData
    {
        public HtmlNode Document { get; set; }
        public string Focuskeyword { get; set; }
        public string Url { get; set; }
    }
}
