using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class HtmlAnalyzer
    {
        private readonly HtmlResult _htmlResult;

        public HtmlAnalyzer(HtmlResult htmlResult)
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

            var imagesAnalyzer = new ImageTagAnalyzer();
            analysis.Results.Add(imagesAnalyzer.Analyse(_htmlResult.Document));

            var deprecatedTagAnalyzer = new DeprecatedTagAnalyzer();
            analysis.Results.Add(deprecatedTagAnalyzer.Analyse(_htmlResult.Document));

            var headerTagAnalyzer = new HeadingAnalyzer();
            analysis.Results.Add(headerTagAnalyzer.Analyse(_htmlResult.Document));

            return analysis;
        }
    }
}
