using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class HtmlTagHelper : IHtmlTagHelper
    {
        public HtmlNode GetHeadTag(HtmlNode document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            return GetSingleTag(document, "head");
        }

        public HtmlNode GetBodyTag(HtmlNode document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            return GetSingleTag(document, "body");
        }

        public HtmlNode GetTitleTag(HtmlNode document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            return GetSingleTag(document, "title");
        }

        public IEnumerable<HtmlNode> GetMetaTags(HtmlNode document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            return GetMultipleTags(document, "meta");
        }

        private HtmlNode GetSingleTag(HtmlNode document, string tagName)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));

            var tags = document.GetElements(tagName);
            if (!tags.Any())
            {
                throw new NoElementFoundException(tagName);
            }
            else if (tags.Count() > 1)
            {
                throw new MultipleElementsFoundException(tagName);
            }
            else
            {
                return tags.FirstOrDefault();
            }
        }

        private IEnumerable<HtmlNode> GetMultipleTags(HtmlNode document, string tagName)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));

            var tags = document.GetElements(tagName);
            if (!tags.Any())
            {
                throw new NoElementFoundException(tagName);
            }
            else
            {
                return tags;
            }
        }
    }
}