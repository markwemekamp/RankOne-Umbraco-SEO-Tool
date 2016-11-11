using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "metakeywordanalyzer")]
    public class MetaKeywordAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "metakeywordanalyzer"
            };

            var metaTags = pageData.Document.GetDescendingElements("meta");

            if (!metaTags.Any())
            {
                result.AddResultRule("metakeywordanalyzer_no_meta_tags", ResultType.Error);
            }
            else
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
                result.AddResultRule("metakeywordanalyzer_no_meta_keywords_tag", ResultType.Hint);
            }
            else if (attributeValues.Count() > 1)
            {
                result.AddResultRule("metakeywordanalyzer_multiple_meta_keywords_tags", ResultType.Warning);
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
                result.AddResultRule("metakeywordanalyzer_no_keywords_value", ResultType.Hint);
            }
            else
            {
                result.AddResultRule("metakeywordanalyzer_keywords_set", ResultType.Success);
            }
        }
    }
}
