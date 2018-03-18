using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.ExtensionMethods
{
    public static class HtmlNodeExtensions
    {
        public static IEnumerable<HtmlNode> GetElements(this HtmlNode htmlNode, string elementName)
        {
            if (elementName == null) throw new ArgumentNullException(nameof(elementName));

            return htmlNode.DescendantsAndSelf().Where(d => d.Name == elementName);
        }

        public static IEnumerable<HtmlNode> GetElementsWithAttribute(this HtmlNode document, string elementName, string attribute)
        {
            if (elementName == null) throw new ArgumentNullException(nameof(elementName));
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            return document.GetElements(elementName).Where(d => d.HasAttributes && d.Attributes.Any(x => x.Name == attribute));
        }

        public static HtmlAttribute GetAttribute(this HtmlNode element, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return element.Attributes.FirstOrDefault(x => x.Name == name);
        }
    }
}