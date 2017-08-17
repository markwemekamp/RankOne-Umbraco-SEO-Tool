using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System.Net;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageAnalysisService
    {
        private readonly ScoreService _scoreService;
        private readonly HtmlHelper _htmlHelper;
        private readonly ByteSizeHelper _byteSizeHelper;
        private readonly IConfigurationHelper _configurationHelper;

        public PageAnalysisService()
        {
            _scoreService = new ScoreService();
            _htmlHelper = new HtmlHelper();     
            _byteSizeHelper = new ByteSizeHelper();
            _configurationHelper = new ConfigurationHelper();
        }

        public PageAnalysis CreatePageAnalysis(IPublishedContent node, string focusKeyword)
        {
            var pageAnalysis = new PageAnalysis();

            try
            {
                var htmlString = _htmlHelper.GetTemplateHtml(node);
                var htmlResult = _htmlHelper.GetHtmlResult(htmlString);

                pageAnalysis.Url = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = _byteSizeHelper.GetByteSize(htmlString);

                SetAnalyzerResults(pageAnalysis, htmlResult);
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            return pageAnalysis;
        }

        private void SetAnalyzerResults(PageAnalysis pageAnalysis, HtmlResult html)
        {
            var summaries = _configurationHelper.GetSummaries();

            // Instantiate the types and retrieve te results
            foreach (var summary in summaries)
            {
                summary.FocusKeyword = pageAnalysis.FocusKeyword;
                summary.HtmlResult = html;
                summary.Url = pageAnalysis.Url;

                var analyzerResult = new AnalyzerResult
                {
                    Alias = summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.AnalyzerResults.Add(analyzerResult);
            }
        }
    }
}