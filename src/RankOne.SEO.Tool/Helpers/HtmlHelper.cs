using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
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
            _contentHelper = templateHelper;
        }

        public HtmlNode GetHtmlNodeFromString(string htmlString)
        {
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
            return _contentHelper.GetNodeHtml(node);
        }
    }
}