using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Analyzers.Keywords
{
    /// <summary>
    /// Analyzer for checking keyword in title tag related optimizations
    ///
    /// https://moz.com/learn/seo/title-tag
    ///
    /// 1. check for title tag - critical
    /// 2. check for multiple title tags - critical
    /// 3. check if title contains keyword - major
    /// 4. location of the keyword - minor
    /// </summary>
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;
        private readonly IOptionHelper _optionHelper;

        private int? _idealKeywordPosition;
        private int IdealKeywordPosition
        {
            get
            {
                if (!_idealKeywordPosition.HasValue)
                {
                    _idealKeywordPosition = _optionHelper.GetOptionValue(Options, "IdealKeywordPosition", 10);
                }
                return _idealKeywordPosition.Value;
            }
        }

        public KeywordTitleAnalyzer() : this(RankOneContext.Instance)
        { }

        public KeywordTitleAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value, rankOneContext.OptionHelper.Value)
        { }

        public KeywordTitleAnalyzer(IHtmlTagHelper htmlTagHelper, IOptionHelper optionHelper)
        {
            _htmlTagHelper = htmlTagHelper;
            _optionHelper = optionHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var titleTag = _htmlTagHelper.GetTitleTag(pageData.Document, result);
            if (titleTag != null)
            {
                var titleText = titleTag.InnerText;
                var position = titleText.IndexOf(pageData.Focuskeyword, StringComparison.InvariantCultureIgnoreCase);

                if (position >= 0)
                {
                    if (position < IdealKeywordPosition)
                    {
                        result.AddResultRule("title_contains_keyword", ResultType.Success);
                    }
                    else
                    {
                        result.AddResultRule("title_not_in_front", ResultType.Hint);
                    }
                }
                else
                {
                    result.AddResultRule("title_doesnt_contain_keyword", ResultType.Warning);
                }
            }

            return result;
        }
    }
}