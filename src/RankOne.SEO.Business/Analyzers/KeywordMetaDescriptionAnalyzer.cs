using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class KeywordMetaDescriptionAnalyzer : BaseAnalyzer
    {
        public KeywordMetaDescriptionAnalyzer()
        {
            Alias = "keywordmetadescriptionanalyzer";
        }

        public AnalyzeResult Analyse(HtmlNode document, string keyword)
        {
            var result = new AnalyzeResult();
            result.Alias = Alias;

            var metaTags = HtmlHelper.GetElements(document, "meta");

            if (!metaTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("no meta tags"), Type = ResultType.Error });
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
                    result.ResultRules.Add(new ResultRule { Code = GetTag("no meta description tag"), Type = ResultType.Warning });
                }
                else if (attributeValues.Count() > 1)
                {
                    result.ResultRules.Add(new ResultRule { Code = GetTag("multiple meta description tags"), Type = ResultType.Warning });
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
                                Code = GetTag("meta description contains keyword"),
                                Type = ResultType.Success
                            });
                        }
                        else
                        {
                            result.ResultRules.Add(new ResultRule
                            {
                                Code = GetTag("meta description doesnt contain keyword"),
                                Type = ResultType.Hint
                            });
                        }
                    }
                }
            }
            return result;
        }

        public override AnalyzeResult Analyse(HtmlNode document)
        {
            throw new NotImplementedException();
        }
    }
}
