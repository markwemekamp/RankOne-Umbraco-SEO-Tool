using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class MetaDescriptionAnalyzer : BaseAnalyzer
    {
        public MetaDescriptionAnalyzer()
        {
            Alias = "metadescriptionanalyzer";
        }

        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = TitleTag;

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("no meta tags"), Type = ResultType.Error});
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
                    result.ResultRules.Add(new ResultRule { Code = GetTag("no meta description tag"), Type = ResultType.Error });
                }
                else if (attributeValues.Count() > 1)
                {
                    result.ResultRules.Add(new ResultRule { Code = GetTag("multiple meta description tags"), Type = ResultType.Error });
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (string.IsNullOrWhiteSpace(descriptionValue))
                        {
                            result.ResultRules.Add(new ResultRule { Code = GetTag("no description value"), Type = ResultType.Error });
                        }
                        else
                        {
                            descriptionValue = descriptionValue.Trim();

                            if (descriptionValue.Length > 160)
                            {
                                result.ResultRules.Add(new ResultRule { Code = GetTag("description too long"), Type = ResultType.Warning });
                            }

                            if (descriptionValue.Length < 50)
                            {
                                result.ResultRules.Add(new ResultRule { Code = GetTag("description too short"), Type = ResultType.Warning });
                            }

                            if (descriptionValue.Length <= 160 && descriptionValue.Length >= 50)
                            {
                                result.ResultRules.Add(new ResultRule { Code = GetTag("description more than 50 less than 160"), Type = ResultType.Succes });
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
