using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Helpers
{
    public class HtmlHelper : IHtmlHelper
    {
        public HtmlResult GetHtmlResult(string htmlString)
        {
            if (htmlString == null) throw new ArgumentNullException(nameof(htmlString));

            var htmlNode = GetHtmlNodeFromString(htmlString);
            var htmlResult = new HtmlResult
            {
                Html = htmlString,
                Document = htmlNode
            };
            return htmlResult;
        }

        private HtmlNode GetHtmlNodeFromString(string htmlString)
        {
            if (htmlString != null)
            {
                var document = new HtmlDocument();
                document.LoadHtml(htmlString);
                return document.DocumentNode;
            }
            return null;
        }
    }
}