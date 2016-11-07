using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace RankOne.ExtensionMethods
{
    public static class HtmlNodeExtensions
    {
        public static IEnumerable<HtmlNode> GetDescendingElements(this HtmlNode htmlNode, string elementName)
        {
            return htmlNode.DescendantsAndSelf().Where(d => d.Name == elementName);
        }

        public static IEnumerable<HtmlNode> GetDescendingElementsWithAttribute(this HtmlNode document, string elementName, string attribute)
        {
            return document.GetDescendingElements(elementName).Where(d => d.HasAttributes && d.Attributes.Any(x => x.Name == attribute));
        }

        public static HtmlAttribute GetAttribute(this HtmlNode element, string name)
        {
            return element.Attributes.FirstOrDefault(x => x.Name == name);
        }

    }
}
