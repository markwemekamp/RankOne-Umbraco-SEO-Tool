using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class MetaDescriptionAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult
            {
                Alias = "metadescriptionanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_no_meta_tags", Type = ResultType.Error});
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
                    result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_no_meta_description_tag", Type = ResultType.Error });
                }
                else if (attributeValues.Count() > 1)
                {
                    result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_multiple_meta_description_tags", Type = ResultType.Error });
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (string.IsNullOrWhiteSpace(descriptionValue))
                        {
                            result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_no_description_value", Type = ResultType.Error });
                        }
                        else
                        {
                            descriptionValue = descriptionValue.Trim();

                            if (descriptionValue.Length > 160)
                            {
                                result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_description_too_long", Type = ResultType.Warning });
                            }

                            if (descriptionValue.Length < 50)
                            {
                                result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_description_too_short", Type = ResultType.Warning });
                            }

                            if (descriptionValue.Length <= 160 && descriptionValue.Length >= 50)
                            {
                                result.ResultRules.Add(new ResultRule { Code = "metadescriptionanalyzer_description_more_than_50_less_than_160", Type = ResultType.Success });
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
