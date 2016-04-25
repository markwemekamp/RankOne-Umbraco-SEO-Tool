using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Keywords
{
    public class KeywordMetaDescriptionAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordmetadescriptionanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");
            var keyword = additionalValues[0].ToString();

            if (!metaTags.Any())
            {
                result.AddResultRule("keywordmetadescriptionanalyzer_no_meta_tags", ResultType.Error);
            }
            else
            {
                var attributeValues = from metaTag in metaTags
                                      let attribute = HtmlHelper.GetAttribute(metaTag, "name")
                                      where attribute != null
                                      where attribute.Value == "description"
                                      select HtmlHelper.GetAttribute(metaTag, "content");

                if (!attributeValues.Any())
                {
                    result.AddResultRule("keywordmetadescriptionanalyzer_no_meta_description_tag", ResultType.Warning);
                }
                else if (attributeValues.Count() > 1)
                {
                    result.AddResultRule("keywordmetadescriptionanalyzer_multiple_meta_description_tags", ResultType.Warning);
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (descriptionValue.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            result.AddResultRule("keywordmetadescriptionanalyzer_meta_description_contains_keyword", ResultType.Success);
                        }
                        else
                        {
                            result.AddResultRule("keywordmetadescriptionanalyzer_meta_description_doesnt_contain_keyword", ResultType.Hint);
                        }
                    }
                }
            }
            return result;
        }
    }
}
