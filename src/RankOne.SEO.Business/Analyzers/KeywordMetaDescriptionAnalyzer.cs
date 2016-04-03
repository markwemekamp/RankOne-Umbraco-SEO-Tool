using System;
using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class KeywordMetaDescriptionAnalyzer : BaseAnalyzer
    {
        public AnalyzeResult Analyse(XDocument document, string keyword)
        {
            var result = new AnalyzeResult();
            result.Title = "keywordmetadescriptionanalyzer_title";

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "keywordmetadescriptionanalyzer_no_meta_tags", Type = ResultType.Error });
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
                    result.ResultRules.Add(new ResultRule { Code = "keywordmetadescriptionanalyzer_no_meta_description_tag", Type = ResultType.Warning });
                }
                else if (attributeValues.Count() > 1)
                {
                    result.ResultRules.Add(new ResultRule { Code = "keywordmetadescriptionanalyzer_multiple_meta_description_tags", Type = ResultType.Warning });
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (descriptionValue.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            result.ResultRules.Add(new ResultRule
                            {
                                Code = "keywordmetadescriptionanalyzer_metadescription_contains_keyword",
                                Type = ResultType.Succes
                            });
                        }
                        else
                        {
                            result.ResultRules.Add(new ResultRule
                            {
                                Code = "keywordmetadescriptionanalyzer_metadescription_doesnt_contain_keyword",
                                Type = ResultType.Hint
                            });
                        }
                    }
                }
            }
            return result;
        }

        public override AnalyzeResult Analyse(XDocument document)
        {
            throw new System.NotImplementedException();
        }
    }
}
