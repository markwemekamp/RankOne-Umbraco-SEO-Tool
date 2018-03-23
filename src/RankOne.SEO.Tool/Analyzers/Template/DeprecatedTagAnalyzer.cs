using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        private readonly IOptionHelper _optionHelper;

        private IEnumerable<string> _deprecatedTags;

        public IEnumerable<string> DeprecatedTags
        {
            get
            {
                if (_deprecatedTags == null)
                {
                    var keywords = _optionHelper.GetOptionValue(Options, "DeprecatedTags", "acronym, applet, basefont, big, center, dir, font, frame, frameset, noframes, strike, tt");

                    _deprecatedTags = keywords.Split(',').Select(x => x.Trim());
                }
                return _deprecatedTags;
            }
        }

        public DeprecatedTagAnalyzer() : this(RankOneContext.Instance)
        { }

        public DeprecatedTagAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.OptionHelper.Value)
        { }

        public DeprecatedTagAnalyzer(IOptionHelper optionHelper) : base()
        {
            if (optionHelper == null) throw new ArgumentNullException(nameof(optionHelper));

            _optionHelper = optionHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            foreach (var deprecatedKeyword in DeprecatedTags)
            {
                CheckTag(pageData.Document, deprecatedKeyword);
            }

            if (!AnalyzeResult.ResultRules.Any())
            {
                AddResultRule("no_deprecated_tags_found", ResultType.Success);
            }
        }

        private void CheckTag(HtmlNode htmlNode, string tagname)
        {
            var acronymTags = htmlNode.GetElements(tagname);

            if (acronymTags.Any())
            {
                var resultRute = new ResultRule()
                {
                    Alias = "tag_found",
                    Type = ResultType.Warning,
                    Tokens = new List<string>() { tagname }
                };

                AddResultRule(resultRute);
            }
        }
    }
}