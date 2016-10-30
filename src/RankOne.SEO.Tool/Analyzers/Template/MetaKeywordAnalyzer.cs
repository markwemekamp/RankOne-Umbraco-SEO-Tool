using System.Linq;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "metakeywordanalyzer")]
    public class MetaKeywordAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(PageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "metakeywordanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(pageData.Document, "meta");

            if (!metaTags.Any())
            {
                result.AddResultRule("metakeywordanalyzer_no_meta_tags", ResultType.Error);
            }
            else
            {
                var attributeValues = from metaTag in metaTags
                                      let attribute = HtmlHelper.GetAttribute(metaTag, "name")
                                      where attribute != null
                                      where attribute.Value == "keywords"
                                      select HtmlHelper.GetAttribute(metaTag, "content");

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
                        var keywordsValue = firstMetaKeywordsTag.Value;

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
            return result;
        }
    }
}
