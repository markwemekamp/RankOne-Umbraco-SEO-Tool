using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    public class GZipAnalyzer : BaseAnalyzer
    {
        private readonly IEncodingHelper _encodingHelper;

        public GZipAnalyzer() : this(RankOneContext.Instance)
        { }

        public GZipAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.EncodingHelper.Value)
        { }

        public GZipAnalyzer(IEncodingHelper encodingHelper)
        {
            _encodingHelper = encodingHelper;
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