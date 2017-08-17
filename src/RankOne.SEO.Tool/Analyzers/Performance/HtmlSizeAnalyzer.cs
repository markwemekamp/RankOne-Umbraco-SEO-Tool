using RankOne.Attributes;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    [AnalyzerCategory(SummaryName = "Performance", Alias = "htmlsizeanalyzer")]
    public class HtmlSizeAnalyzer : BaseAnalyzer
    {
        private readonly IByteSizeHelper _byteSizeHelper;

        private static readonly int MaximumSizeInKb = 33792;   // 33 kb

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

            if (byteCount < MaximumSizeInKb)
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