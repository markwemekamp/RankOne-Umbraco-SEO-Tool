using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using Umbraco.Core.Models;

namespace RankOne.Helpers
{
    public class HtmlHelper : IHtmlHelper
    {
        private readonly ITemplateHelper _contentHelper;

        public HtmlHelper() : this(RankOneContext.Instance)
        { }

        public HtmlHelper(RankOneContext rankOneContext) : this(rankOneContext.TemplateHelper.Value)
        { }

        public HtmlHelper(ITemplateHelper templateHelper)
        {
            if (templateHelper == null) throw new ArgumentNullException(nameof(templateHelper));

            _contentHelper = templateHelper;
        }

        public HtmlNode GetHtmlNodeFromString(string htmlString)
        {
            if (htmlString == null) throw new ArgumentNullException(nameof(htmlString));

            if (htmlString != null)
            {
                var document = new HtmlDocument();
                document.LoadHtml(htmlString);
                return document.DocumentNode;
            }
            return null;
        }

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

        public string GetTemplateHtml(IPublishedContent node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return _contentHelper.GetNodeHtml(node);
        }
    }
}