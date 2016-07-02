using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace RankOne.Helpers
{
    public class HtmlHelper
    {
        public IEnumerable<HtmlNode> GetElements(HtmlNode document, string elementName)
        {
            return document.DescendantsAndSelf()
                   .Where(d => d.Name == elementName);
        }

        public IEnumerable<HtmlNode> GetElementsWithAttribute(HtmlNode document, string elementName, string attribute)
        {
            return GetElements(document, elementName).Where(d => d.HasAttributes && d.Attributes.Any(x => x.Name == attribute));
        }

        public HtmlAttribute GetAttribute(HtmlNode element, string name)
        {
            return element.Attributes.FirstOrDefault(x => x.Name == name);
        }
    }
}
