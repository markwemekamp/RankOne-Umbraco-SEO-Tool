using System.Net;
using System.Text;
using HtmlAgilityPack;
using RankOne.Business.Models;
using RankOne.Business.Summaries;
using Umbraco.Web;

namespace RankOne.Business.Services
{
    public class AnalyzeService
    {
        private readonly HtmlDocument _htmlParser;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
        }

        public PageAnalysis AnalyzeWebPage(int id, string focusKeyword)
        {
            var webpage = new PageAnalysis();
            try
            {
                webpage.HtmlResult = GetHtml(id);

                var keywordAnalyzer = new KeywordSummary(webpage.HtmlResult);
                keywordAnalyzer.FocusKeyword = focusKeyword;
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "keywordanalyzer",
                    Analysis = keywordAnalyzer.GetAnalysis()
                });

                var htmlAnalyzer = new HtmlSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "htmlanalyzer",
                    Analysis = htmlAnalyzer.GetAnalysis()
                });

                var performanceAnalyzer = new PerformanceSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "performanceanalyzer",
                    Analysis = performanceAnalyzer.GetAnalysis()
                });
            }
            catch (WebException ex)
            {
                webpage.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }
            return webpage;
        }

        private HtmlResult GetHtml(int id)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var node = umbracoHelper.TypedContent(id);
            var htmlObject = umbracoHelper.RenderTemplate(id);
            var html = htmlObject.ToHtmlString();

            _htmlParser.LoadHtml(html);

            return new HtmlResult
            {
                Url = node.UrlAbsolute(),
                Html = html,
                Size = Encoding.ASCII.GetByteCount(html),
                Document = _htmlParser.DocumentNode
            };
        }
    }
}
