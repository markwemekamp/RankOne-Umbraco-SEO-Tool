using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class HtmlSizeAnalyzer : BaseAnalyzer
    {
        private readonly IByteSizeHelper _byteSizeHelper;
        private readonly IOptionHelper _optionHelper;

        private int? _maximumSizeInBytes;
        private int MaximumSizeInBytes
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

        public HtmlSizeAnalyzer(IByteSizeHelper byteSizeHelper, IOptionHelper optionHelper)
        {
            _byteSizeHelper = byteSizeHelper;
            _optionHelper = optionHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var byteCount = _byteSizeHelper.GetByteSize(pageData.Document.InnerHtml);
            var htmlSizeResultRule = new ResultRule();

            if (byteCount < MaximumSizeInBytes)
            {
                htmlSizeResultRule.Alias = "html_size_small";
                htmlSizeResultRule.Type = ResultType.Success;
            }
            else
            {
                htmlSizeResultRule.Alias = "htmlsizeanalyzer_html_size_too_large";
                htmlSizeResultRule.Type = ResultType.Warning;
            }
            htmlSizeResultRule.Tokens.Add(_byteSizeHelper.GetSizeSuffix(byteCount));
            htmlSizeResultRule.Tokens.Add(_byteSizeHelper.GetSizeSuffix(MaximumSizeInBytes));
            result.ResultRules.Add(htmlSizeResultRule);

            return result;
        }
    }
}