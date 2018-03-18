﻿using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class HtmlTagHelper : IHtmlTagHelper
    {
        public HtmlNode GetHeadTag(HtmlNode document, AnalyzeResult result)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));

            return GetSingleTag(document, result, "head");
        }

        public HtmlNode GetBodyTag(HtmlNode document, AnalyzeResult result)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));

            return GetSingleTag(document, result, "body");
        }

        public HtmlNode GetTitleTag(HtmlNode document, AnalyzeResult result)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));

            return GetSingleTag(document, result, "title");
        }

        public IEnumerable<HtmlNode> GetMetaTags(HtmlNode document, AnalyzeResult result)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));

            return GetMultipleTags(document, result, "meta");
        }

        private HtmlNode GetSingleTag(HtmlNode document, AnalyzeResult result, string tagName)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));

            var tags = document.GetElements(tagName);
            if (!tags.Any())
            {
                result.AddResultRule("no_" + tagName + "_tag", ResultType.Error);
            }
            else if (tags.Count() > 1)
            {
                result.AddResultRule("multiple_" + tagName + "_tags", ResultType.Error);
            }
            else
            {
                return tags.FirstOrDefault();
            }
            return null;
        }

        private IEnumerable<HtmlNode> GetMultipleTags(HtmlNode document, AnalyzeResult result, string tagName)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));

            var tags = document.GetElements(tagName);
            if (!tags.Any())
            {
                result.AddResultRule("no_" + tagName + "_tags", ResultType.Error);
            }
            else
            {
                return tags;
            }
            return null;
        }
    }
}