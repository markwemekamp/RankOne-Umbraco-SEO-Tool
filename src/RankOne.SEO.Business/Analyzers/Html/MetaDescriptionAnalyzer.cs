using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
{
    public class MetaDescriptionAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "metadescriptionanalyzer"
            };

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.AddResultRule("metadescriptionanalyzer_no_meta_tags", ResultType.Error);
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
                    result.AddResultRule("metadescriptionanalyzer_no_meta_description_tag", ResultType.Error);
                }
                else if (attributeValues.Count() > 1)
                {
                    result.AddResultRule("metadescriptionanalyzer_multiple_meta_description_tags", ResultType.Error);
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (string.IsNullOrWhiteSpace(descriptionValue))
                        {
                            result.AddResultRule("metadescriptionanalyzer_no_description_value", ResultType.Error);
                        }
                        else
                        {
                            descriptionValue = descriptionValue.Trim();

                            if (descriptionValue.Length > 155)
                            {
                                result.AddResultRule("metadescriptionanalyzer_description_too_long", ResultType.Warning);
                            }

                            if (descriptionValue.Length < 50)
                            {
                                result.AddResultRule("metadescriptionanalyzer_description_too_short", ResultType.Warning);
                            }

                            if (descriptionValue.Length < 130)
                            {
                                result.AddResultRule("metadescriptionanalyzer_description_too_short", ResultType.Hint);
                            }

                            if (descriptionValue.Length <= 155 && descriptionValue.Length >= 130)
                            {
                                result.AddResultRule("metadescriptionanalyzer_description_perfect", ResultType.Success);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
