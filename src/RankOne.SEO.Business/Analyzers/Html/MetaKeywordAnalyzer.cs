using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
{
    public class MetaKeywordAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "metakeywordanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");

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
