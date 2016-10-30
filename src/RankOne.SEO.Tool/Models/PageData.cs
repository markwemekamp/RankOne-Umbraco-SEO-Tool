using HtmlAgilityPack;

namespace RankOne.Models
{
    public class PageData
    {
        public HtmlNode Document { get; set; }
        public string Focuskeyword { get; set; }
        public string Url { get; set; }
    }
}
