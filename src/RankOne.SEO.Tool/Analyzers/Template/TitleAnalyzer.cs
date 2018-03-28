using HtmlAgilityPack;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Exceptions;
using System;

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

        public int MaximumLength
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

        private int? _minimumLength;

        public int MinimumLength
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

        public TitleAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value, rankOneContext.OptionHelper.Value)
        { }

        public TitleAnalyzer(IHtmlTagHelper htmlTagHelper, IOptionHelper optionHelper) : base()
        {
            if (htmlTagHelper == null) throw new ArgumentNullException(nameof(htmlTagHelper));
            if (optionHelper == null) throw new ArgumentNullException(nameof(optionHelper));

            _htmlTagHelper = htmlTagHelper;
            _optionHelper = optionHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            try
            {
                var headTag = _htmlTagHelper.GetHeadTag(pageData.Document);
                AnalyzeHeadTag(headTag);
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Error);
            }
            catch (MultipleElementsFoundException e)
            {
                AddResultRule("multiple_" + e.ElementName + "_tags", ResultType.Error);
            }
        }

        private void AnalyzeHeadTag(HtmlNode headTag)
        {
            try
            {
                var titleTag = _htmlTagHelper.GetTitleTag(headTag);
                AnalyzeTitleTag(titleTag);
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Error);
            }
            catch (MultipleElementsFoundException e)
            {
                AddResultRule("multiple_" + e.ElementName + "_tags", ResultType.Error);
            }
        }

        private void AnalyzeTitleTag(HtmlNode titleTag)
        {
            var titleValue = titleTag.InnerText;

            var resultRule = new ResultRule();

            if (string.IsNullOrWhiteSpace(titleValue))
            {
                resultRule.Alias = "no_title_value";
                resultRule.Type = ResultType.Error;
            }
            else
            {
                titleValue = titleValue.Trim();

                if (titleValue.Length > MaximumLength)
                {
                    resultRule.Alias = "title_too_long";
                    resultRule.Type = ResultType.Warning;
                }

                if (titleValue.Length < MinimumLength)
                {
                    resultRule.Alias = "title_too_short";
                    resultRule.Type = ResultType.Hint;
                }

                if (titleValue.Length <= MaximumLength && titleValue.Length >= MinimumLength)
                {
                    resultRule.Alias = "title_success";
                    resultRule.Type = ResultType.Success;
                }
            }

            resultRule.Tokens.Add(MaximumLength.ToString());        // 0
            resultRule.Tokens.Add(MinimumLength.ToString());        // 1

            AddResultRule(resultRule);
        }
    }
}