using System.Collections.Generic;
using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;

namespace RankOne.Models
{
    public class PageData : IPageData
    {
        public HtmlNode Document { get; set; }
        public string Focuskeyword { get; set; }
        public string Url { get; set; }

        public IEnumerable<HtmlNode> GetElements(string name)
        {
            return Document.GetDescendingElements(name);
        }
    }
}
