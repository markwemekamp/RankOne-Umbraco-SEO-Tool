using System.Text;
using HtmlAgilityPack;
using RankOne.Models;
using Umbraco.Core.Models;

namespace RankOne.Helpers
{
    public class HtmlHelper
    {
        private readonly HtmlDocument _htmlParser;
        private readonly TemplateHelper _contentHelper;

        public HtmlHelper()
        {
            _htmlParser = new HtmlDocument();
            _contentHelper = new TemplateHelper();
        }

        public int GetSize(string htmlString)
        {
            if (htmlString == null)
            {
                return 0;
            }
            return Encoding.ASCII.GetByteCount(htmlString);
        }

        public HtmlNode GetHtmlNodeFromString(string htmlString)
        {
            if (htmlString != null)
            {
                _htmlParser.LoadHtml(htmlString);
                return _htmlParser.DocumentNode;
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
