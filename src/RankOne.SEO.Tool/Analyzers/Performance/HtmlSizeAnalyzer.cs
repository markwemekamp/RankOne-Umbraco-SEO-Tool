using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class HtmlSizeAnalyzer : BaseAnalyzer
    {
        private readonly IByteSizeHelper _byteSizeHelper;

        private int? _maximumSizeInBytes;
        private int MaximumSizeInBytes
        {
            get
            {
                if (!_maximumSizeInBytes.HasValue)
                {
                    var option = Options.FirstOrDefault(x => x.Key == "MaximumSizeInBytes");
                    var optionValue = 0;
                    if (option != null)
                    {
                        int.TryParse(option.Value, out optionValue);
                    }
                    _maximumSizeInBytes = optionValue > 0 ? optionValue : 33792;
                }
                return _maximumSizeInBytes.Value;
            }
        }

        public HtmlSizeAnalyzer() : this(RankOneContext.Instance)
        { }

        public HtmlSizeAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.ByteSizeHelper.Value)
        { }

        public HtmlSizeAnalyzer(IByteSizeHelper byteSizeHelper)
        {
            _byteSizeHelper = byteSizeHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var htmlSizeAnalysis = new AnalyzeResult();

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
            htmlSizeAnalysis.ResultRules.Add(htmlSizeResultRule);

            return htmlSizeAnalysis;
        }
    }
}