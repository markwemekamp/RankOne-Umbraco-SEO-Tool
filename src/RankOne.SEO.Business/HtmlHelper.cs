using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RankOne.Business
{
    public class HtmlHelper
    {
        public IEnumerable<XElement> GetElements(XDocument document, string elementName)
        {
            return GetElements(document.Root, elementName);
        }

        public IEnumerable<XElement> GetElements(XElement element, string elementName)
        {
            return element.DescendantsAndSelf().Elements()
                   .Where(d => d.Name.LocalName == elementName);
        }

        public IEnumerable<XElement> GetElementsWithAttribute(XDocument document, string elementName, string attribute)
        {
            return GetElements(document, elementName).Where(d => d.HasAttributes && d.Attributes().Any(x => x.Name == attribute));
        }

        public XAttribute GetAttribute(XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(x => x.Name.LocalName == name);
        }
    }
}
