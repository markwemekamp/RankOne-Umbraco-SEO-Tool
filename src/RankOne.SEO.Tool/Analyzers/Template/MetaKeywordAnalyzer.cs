using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Helpers;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "metakeywordanalyzer")]
    public class MetaKeywordAnalyzer : BaseAnalyzer
    {
        private readonly HtmlTagHelper _htmlTagHelper;

        public MetaKeywordAnalyzer()
        {
            _htmlTagHelper = new HtmlTagHelper();
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document, result);

            if (metaTags.Any())
            {
                AnalyzeMetaTags(metaTags, result);
            }
            return result;
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags, AnalyzeResult result)
        {
            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "keywords"
                                  select metaTag.GetAttribute("content");

            if (!attributeValues.Any())
            {
                result.AddResultRule("no_meta_keywords_tag", ResultType.Hint);
            }
            else if (attributeValues.Count() > 1)
            {
                result.AddResultRule("multiple_meta_keywords_tags", ResultType.Warning);
            }
            else
            {
                var firstMetaKeywordsTag = attributeValues.FirstOrDefault();
                if (firstMetaKeywordsTag != null)
                {
                    AnalyzeMetaKeywordsAttribute(firstMetaKeywordsTag, result);
                }
            }
        }

        private void AnalyzeMetaKeywordsAttribute(HtmlAttribute metaKeywordsTag, AnalyzeResult result)
        {
            var keywordsValue = metaKeywordsTag.Value;

            if (string.IsNullOrWhiteSpace(keywordsValue))
            {
                result.AddResultRule("no_keywords_value", ResultType.Hint);
            }
            else
            {
                result.AddResultRule("keywords_set", ResultType.Success);
            }
        }
    }
}
