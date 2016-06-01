using RankOne.Business.Analyzers.Html;
using RankOne.Business.Models;

namespace RankOne.Business.Summaries
{
    public class HtmlSummary
    {
        private readonly HtmlResult _htmlResult;

        public HtmlSummary(HtmlResult htmlResult)
        {
            _htmlResult = htmlResult;
        }

        public Analysis GetAnalysis()
        {
            var analysis = new Analysis();

            var titleAnalyzer = new TitleAnalyzer();
            analysis.Results.Add(titleAnalyzer.Analyse(_htmlResult.Document));

            var metaDescriptionAnalyzer = new MetaDescriptionAnalyzer();
            analysis.Results.Add(metaDescriptionAnalyzer.Analyse(_htmlResult.Document));

            var metaKeywordAnalyzer = new MetaKeywordAnalyzer();
            analysis.Results.Add(metaKeywordAnalyzer.Analyse(_htmlResult.Document));

            var imagesAnalyzer = new ImageTagAnalyzer();
            analysis.Results.Add(imagesAnalyzer.Analyse(_htmlResult.Document));

            var anchorAnalyzer = new AnchorTagAnalyzer();
            analysis.Results.Add(anchorAnalyzer.Analyse(_htmlResult.Document));

            var deprecatedTagAnalyzer = new DeprecatedTagAnalyzer();
            analysis.Results.Add(deprecatedTagAnalyzer.Analyse(_htmlResult.Document));

            var metaRobotsAnalyzer = new MetaRobotsAnalyzer();
            analysis.Results.Add(metaRobotsAnalyzer.Analyse(_htmlResult.Document));

            return analysis;
        }
    }
}
