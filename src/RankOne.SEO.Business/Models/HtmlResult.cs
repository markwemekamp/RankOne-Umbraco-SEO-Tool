using HtmlAgilityPack;

namespace RankOne.Business.Models
{
    public class HtmlResult
    {
        public string Url { get; set; }
        public HtmlNode Document { get; set; }
        public string Html { get; set; }
        public long ServerResponseTime { get; set; }
        public int Size { get; internal set; }
    }
}
