using RankOne.Attributes;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    [AnalyzerCategory(SummaryName = "Performance", Alias = "gzipanalyzer")]
    public class GZipAnalyzer : BaseAnalyzer
    {
        private readonly EncodingHelper _encodingHelper;

        public GZipAnalyzer()
        {
            _encodingHelper = new EncodingHelper();
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var encoding = _encodingHelper.GetEncodingFromUrl(pageData.Url);

            var result = new AnalyzeResult();
            if (encoding == "gzip")
            {
                result.AddResultRule("gzip_enabled", ResultType.Success);
            }
            else
            {
                result.AddResultRule("gzip_disabled", ResultType.Hint);
            }

            return result;
        }
    }
}
