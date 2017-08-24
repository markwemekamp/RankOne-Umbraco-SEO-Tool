using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    /// Analyzer for checking title tag related optimizations
    ///
    /// Sources: https://moz.com/learn/seo/title-tag, SEO for 2016 by Sean Odom
    ///
    /// 1. check for head tag - critical
    /// 2. check for presence of title tag - critical
    /// 3. check for multiple title tags - critical
    /// 4. check for value of title tag - critical
    /// 5. check title tag length
    ///     1. longer than 60 - major
    ///     2. shorter than 10 - major
    /// </summary>
    public class TitleAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;
        private readonly IOptionHelper _optionHelper;

        private int? _maximumLength;
        private int? _minimumLength;

        private int MaximumLength
        {
            get
            {
                if (!_maximumLength.HasValue)
                {
                    _maximumLength = _optionHelper.GetOptionValue(Options, "MaximumLength", 60);
                }
                return _maximumLength.Value;
            }
        }

        private int MinimumLength
        {
            get
            {
                if (!_minimumLength.HasValue)
                {
                    _minimumLength = _optionHelper.GetOptionValue(Options, "MinimumLength", 5);
                }
                return _minimumLength.Value;
            }
        }

        public TitleAnalyzer() : this(RankOneContext.Instance)
        { }

        public TitleAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value, rankOneContext.OptionHelper.Value)
        { }

        public TitleAnalyzer(IHtmlTagHelper htmlTagHelper, IOptionHelper optionHelper)
        {
            _htmlTagHelper = htmlTagHelper;
            _optionHelper = optionHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var headTag = _htmlTagHelper.GetHeadTag(pageData.Document, result);

            if (headTag != null)
            {
                AnalyzeHeadTag(headTag, result);
            }

            return result;
        }

        private void AnalyzeHeadTag(HtmlNode headTag, AnalyzeResult result)
        {
            var titleTag = _htmlTagHelper.GetTitleTag(headTag, result);

            if (titleTag != null)
            {
                AnalyzeTitleTag(titleTag, result);
            }
        }

        private void AnalyzeTitleTag(HtmlNode titleTag, AnalyzeResult result)
        {
            var titleValue = titleTag.InnerText;

            if (string.IsNullOrWhiteSpace(titleValue))
            {
                result.AddResultRule("no_title_value", ResultType.Error);
            }
            else
            {
                titleValue = titleValue.Trim();

                if (titleValue.Length > MaximumLength)
                {
                    result.AddResultRule("title_too_long", ResultType.Warning);
                }

                if (titleValue.Length < MinimumLength)
                {
                    result.AddResultRule("titleanalyzer_title_too_short", ResultType.Hint);
                }

                if (titleValue.Length <= MaximumLength && titleValue.Length >= MinimumLength)
                {
                    result.AddResultRule("title_success", ResultType.Success);
                }
            }
        }
    }
}