using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    ///
    /// Sources: https://moz.com/learn/seo/meta-description, SEO for 2016 by Sean Odom
    ///
    /// TODO
    /// check for quotes
    ///
    /// </summary>
    public class MetaDescriptionAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;
        private readonly IOptionHelper _optionHelper;

        private int? _maximumLength;
        private int MaximumLength
        {
            get
            {
                if (!_maximumLength.HasValue)
                {
                    _maximumLength = _optionHelper.GetOptionValue(Options, "MaximumLength", 150);
                }
                return _maximumLength.Value;
            }
        }

        private int? _minimumLength;
        private int MinimumLength
        {
            get
            {
                if (!_minimumLength.HasValue)
                {
                    _minimumLength = _optionHelper.GetOptionValue(Options, "MinimumLength", 20);
                }
                return _minimumLength.Value;
            }
        }

        private int? _acceptableLength;
        private int AcceptableLength
        {
            get
            {
                if (!_acceptableLength.HasValue)
                {
                    _acceptableLength = _optionHelper.GetOptionValue(Options, "AcceptableLength", 50);
                }
                return _acceptableLength.Value;
            }
        }

        public MetaDescriptionAnalyzer() : this(RankOneContext.Instance)
        { }

        public MetaDescriptionAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value, rankOneContext.OptionHelper.Value)
        { }

        public MetaDescriptionAnalyzer(IHtmlTagHelper htmlTagHelper, IOptionHelper optionHelper)
        {
            _htmlTagHelper = htmlTagHelper;
            _optionHelper = optionHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document, result);

            if (metaTags.Any())
            {
                AnalyzeMetaTags(metaTags, result);
            }
            return result;
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags, AnalyzeResult result)
        {
            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "description"
                                  select metaTag.GetAttribute("content");

            if (!attributeValues.Any())
            {
                result.AddResultRule("no_meta_description_tag", ResultType.Error);
            }
            else if (attributeValues.Count() > 1)
            {
                result.AddResultRule("multiple_meta_description_tags", ResultType.Error);
            }
            else
            {
                var firstMetaDescriptionAttribute = attributeValues.FirstOrDefault();
                if (firstMetaDescriptionAttribute != null)
                {
                    AnalyzeMetaDescriptionAttribute(firstMetaDescriptionAttribute, result);
                }
            }
        }

        private void AnalyzeMetaDescriptionAttribute(HtmlAttribute metaDescriptionAttribute, AnalyzeResult result)
        {
            var descriptionValue = metaDescriptionAttribute.Value;

            if (string.IsNullOrWhiteSpace(descriptionValue))
            {
                result.AddResultRule("no_description_value", ResultType.Error);
            }
            else
            {
                descriptionValue = descriptionValue.Trim();

                if (descriptionValue.Length > MaximumLength)
                {
                    result.AddResultRule("description_too_long", ResultType.Warning);
                }

                if (descriptionValue.Length < MinimumLength)
                {
                    result.AddResultRule("description_too_short", ResultType.Warning);
                }

                if (descriptionValue.Length < AcceptableLength)
                {
                    result.AddResultRule("description_too_short", ResultType.Hint);
                }

                if (descriptionValue.Length <= MaximumLength && descriptionValue.Length >= AcceptableLength)
                {
                    result.AddResultRule("description_perfect", ResultType.Success);
                }
            }
        }
    }
}