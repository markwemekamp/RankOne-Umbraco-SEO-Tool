using RankOne.Interfaces;
using RankOne.Models;
using System.Net;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageAnalysisService : IPageAnalysisService
    {
        private readonly IScoreService _scoreService;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IByteSizeHelper _byteSizeHelper;
        private readonly IConfigurationHelper _configurationHelper;

        public PageAnalysisService() : this(RankOneContext.Instance)
        { }

        public PageAnalysisService(RankOneContext rankOneContext) : this(rankOneContext.ScoreService.Value, rankOneContext.HtmlHelper.Value, rankOneContext.ByteSizeHelper.Value, rankOneContext.ConfigurationHelper.Value)
        { }

        public PageAnalysisService(IScoreService scoreService, IHtmlHelper htmlHelper, IByteSizeHelper byteSizeHelper, IConfigurationHelper configurationHelper)
        {
            _scoreService = scoreService;
            _htmlHelper = htmlHelper;
            _byteSizeHelper = byteSizeHelper;
            _configurationHelper = configurationHelper;
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