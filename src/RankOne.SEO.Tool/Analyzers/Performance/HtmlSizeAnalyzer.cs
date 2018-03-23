using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Analyzers.Performance
{
    public class HtmlSizeAnalyzer : BaseAnalyzer
    {
        private readonly IByteSizeHelper _byteSizeHelper;
        private readonly IOptionHelper _optionHelper;

        private int? _maximumSizeInBytes;

        public int MaximumSizeInBytes
        {
            get
            {
                if (!_maximumSizeInBytes.HasValue)
                {
                    _maximumSizeInBytes = _optionHelper.GetOptionValue(Options, "MaximumSizeInBytes", 33792);
                }
                return _maximumSizeInBytes.Value;
            }
        }

        public HtmlSizeAnalyzer() : this(RankOneContext.Instance)
        { }

        public HtmlSizeAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.ByteSizeHelper.Value, rankOneContext.OptionHelper.Value)
        { }

        public HtmlSizeAnalyzer(IByteSizeHelper byteSizeHelper, IOptionHelper optionHelper) : base()
        {
            if (byteSizeHelper == null) throw new ArgumentNullException(nameof(byteSizeHelper));
            if (optionHelper == null) throw new ArgumentNullException(nameof(optionHelper));

            _byteSizeHelper = byteSizeHelper;
            _optionHelper = optionHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var byteCount = _byteSizeHelper.GetByteSize(pageData.Document.InnerHtml);
            var htmlSizeResultRule = new ResultRule();

            if (byteCount < MaximumSizeInBytes)
            {
                htmlSizeResultRule.Alias = "html_size_small";
                htmlSizeResultRule.Type = ResultType.Success;
            }
            else
            {
                htmlSizeResultRule.Alias = "html_size_too_large";
                htmlSizeResultRule.Type = ResultType.Warning;
            }
            htmlSizeResultRule.Tokens.Add(_byteSizeHelper.GetSizeSuffix(byteCount));
            htmlSizeResultRule.Tokens.Add(_byteSizeHelper.GetSizeSuffix(MaximumSizeInBytes));
            AddResultRule(htmlSizeResultRule);
        }
    }
}