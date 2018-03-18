using HtmlAgilityPack;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IHtmlTagHelper
    {
        HtmlNode GetHeadTag(HtmlNode document);

        HtmlNode GetBodyTag(HtmlNode document);

        HtmlNode GetTitleTag(HtmlNode document);

        IEnumerable<HtmlNode> GetMetaTags(HtmlNode document);
    }
}