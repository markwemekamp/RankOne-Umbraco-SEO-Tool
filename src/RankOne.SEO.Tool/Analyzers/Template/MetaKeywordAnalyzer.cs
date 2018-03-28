using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class MetaKeywordAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;

        public MetaKeywordAnalyzer() : this(RankOneContext.Instance)
        { }

        public MetaKeywordAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public MetaKeywordAnalyzer(IHtmlTagHelper htmlTagHelper) : base()
        {
            if (htmlTagHelper == null) throw new ArgumentNullException(nameof(htmlTagHelper));

            _htmlTagHelper = htmlTagHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            try
            {
                var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document);

                if (metaTags.Any())
                {
                    AnalyzeMetaTags(metaTags);
                }
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Error);
            }
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags)
        {
            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "keywords"
                                  select metaTag.GetAttribute("content");

            if (!attributeValues.Any())
            {
                AddResultRule("no_meta_keywords_tag", ResultType.Hint);
            }
            else if (attributeValues.Count() > 1)
            {
                AddResultRule("multiple_meta_keywords_tags", ResultType.Warning);
            }
            else
            {
                var firstMetaKeywordsTag = attributeValues.FirstOrDefault();
                if (firstMetaKeywordsTag != null)
                {
                    AnalyzeMetaKeywordsAttribute(firstMetaKeywordsTag);
                }
            }
        }

        private void AnalyzeMetaKeywordsAttribute(HtmlAttribute metaKeywordsTag)
        {
            var keywordsValue = metaKeywordsTag.Value;

            if (string.IsNullOrWhiteSpace(keywordsValue))
            {
                AddResultRule("no_keywords_value", ResultType.Hint);
            }
            else
            {
                AddResultRule("keywords_set", ResultType.Success);
            }
        }
    }
}