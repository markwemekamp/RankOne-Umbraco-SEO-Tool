using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Analyzers.Keywords
{
    public class KeywordMetaDescriptionAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;

        public KeywordMetaDescriptionAnalyzer() : this(RankOneContext.Instance)
        { }

        public KeywordMetaDescriptionAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public KeywordMetaDescriptionAnalyzer(IHtmlTagHelper htmlTagHelper)
        {
            _htmlTagHelper = htmlTagHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document, result);

            if (metaTags.Any())
            {
                var attributeValues = from metaTag in metaTags
                                      let attribute = metaTag.GetAttribute("name")
                                      where attribute != null
                                      where attribute.Value == "description"
                                      select metaTag.GetAttribute("content");

                if (!attributeValues.Any())
                {
                    result.AddResultRule("no_meta_description_tag", ResultType.Warning);
                }
                else if (attributeValues.Count() > 1)
                {
                    result.AddResultRule("multiple_meta_description_tags", ResultType.Warning);
                }
                else
                {
                    var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                    if (firstMetaDescriptionTag != null)
                    {
                        var descriptionValue = firstMetaDescriptionTag.Value;

                        if (descriptionValue.IndexOf(pageData.Focuskeyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            result.AddResultRule("meta_description_contains_keyword", ResultType.Success);
                        }
                        else
                        {
                            result.AddResultRule("meta_description_doesnt_contain_keyword", ResultType.Hint);
                        }
                    }
                }
            }
            return result;
        }
    }
}