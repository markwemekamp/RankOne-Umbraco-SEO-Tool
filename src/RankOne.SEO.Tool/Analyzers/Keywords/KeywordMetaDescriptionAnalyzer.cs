using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordmetadescriptionanalyzer")]
    public class KeywordMetaDescriptionAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordmetadescriptionanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");

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

                        if (descriptionValue.IndexOf(focuskeyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
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
