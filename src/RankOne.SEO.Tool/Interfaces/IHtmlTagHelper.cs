using HtmlAgilityPack;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IHtmlTagHelper
    {
        HtmlNode GetHeadTag(HtmlNode document, AnalyzeResult result);

        HtmlNode GetBodyTag(HtmlNode document, AnalyzeResult result);

        HtmlNode GetTitleTag(HtmlNode document, AnalyzeResult result);

        IEnumerable<HtmlNode> GetMetaTags(HtmlNode document, AnalyzeResult result);
    }
}