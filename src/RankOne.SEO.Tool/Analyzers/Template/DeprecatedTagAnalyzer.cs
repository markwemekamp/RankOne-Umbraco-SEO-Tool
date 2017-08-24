using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        private readonly IOptionHelper _optionHelper;

        private IEnumerable<string> _deprecatedTags;
        private IEnumerable<string> DeprecatedTags
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

        public DeprecatedTagAnalyzer(IOptionHelper optionHelper)
        {
            _optionHelper = optionHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            foreach (var deprecatedKeyword in DeprecatedTags)
            {
                CheckTag(pageData.Document, deprecatedKeyword, result);
            } 

            if (!result.ResultRules.Any())
            {
                result.AddResultRule("no_deprecated_tags_found", ResultType.Success);
            }

            return result;
        }

        private void CheckTag(HtmlNode htmlNode, string tagname, AnalyzeResult result)
        {
            var acronymTags = htmlNode.GetElements(tagname);

            if (acronymTags.Any())
            {
                result.AddResultRule(string.Format("{0}_tag_found", tagname), ResultType.Warning);
            }
        }
    }
}