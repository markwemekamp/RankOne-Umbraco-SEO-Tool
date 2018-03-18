using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Exceptions;
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

        public KeywordMetaDescriptionAnalyzer(IHtmlTagHelper htmlTagHelper) : base()
        {
            if (htmlTagHelper == null) throw new ArgumentNullException(nameof(htmlTagHelper));

            _htmlTagHelper = htmlTagHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            try
            {
                var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document);

                if (metaTags.Any())
                {
                    var attributeValues = from metaTag in metaTags
                                          let attribute = metaTag.GetAttribute("name")
                                          where attribute != null
                                          where attribute.Value == "description"
                                          select metaTag.GetAttribute("content");

                    if (!attributeValues.Any())
                    {
                        AddResultRule("no_meta_description_tag", ResultType.Warning);
                    }
                    else if (attributeValues.Count() > 1)
                    {
                        AddResultRule("multiple_meta_description_tags", ResultType.Warning);
                    }
                    else
                    {
                        var firstMetaDescriptionTag = attributeValues.FirstOrDefault();
                        if (firstMetaDescriptionTag != null)
                        {
                            var descriptionValue = firstMetaDescriptionTag.Value;

                            if (descriptionValue.IndexOf(pageData.Focuskeyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                AddResultRule("meta_description_contains_keyword", ResultType.Success);
                            }
                            else
                            {
                                AddResultRule("meta_description_doesnt_contain_keyword", ResultType.Hint);
                            }
                        }
                    }
                }
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Warning);
            }
        }
    }
}