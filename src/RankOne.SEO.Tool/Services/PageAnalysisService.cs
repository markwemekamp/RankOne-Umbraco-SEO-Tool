using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Net;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageAnalysisService : IPageAnalysisService
    {
        private readonly IEnumerable<ISummary> _summaries;
        private readonly IScoreService _scoreService;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IByteSizeHelper _byteSizeHelper;
        private readonly ITemplateHelper _templateHelper;

        public PageAnalysisService() : this(RankOneContext.Instance)
        { }

        public PageAnalysisService(IRankOneContext rankOneContext) : this(rankOneContext.ScoreService.Value, rankOneContext.HtmlHelper.Value, 
            rankOneContext.ByteSizeHelper.Value, rankOneContext.Summaries.Value, rankOneContext.TemplateHelper.Value)
        { }

        public PageAnalysisService(IScoreService scoreService, IHtmlHelper htmlHelper, IByteSizeHelper byteSizeHelper, IEnumerable<ISummary> summaries, 
            ITemplateHelper templateHelper)
        {
            _scoreService = scoreService;
            _htmlHelper = htmlHelper;
            _byteSizeHelper = byteSizeHelper;
            _summaries = summaries;
            _templateHelper = templateHelper;
        }

        public PageAnalysis CreatePageAnalysis(IPublishedContent node, string focusKeyword = null)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var pageAnalysis = new PageAnalysis();

            try
            {
                var htmlString = _templateHelper.GetNodeHtml(node);
                var htmlResult = _htmlHelper.GetHtmlResult(htmlString);

                pageAnalysis.AbsoluteUrl = node.UrlAbsolute();
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
            // Instantiate the types and retrieve te results
            foreach (var summary in _summaries)
            {
                summary.FocusKeyword = pageAnalysis.FocusKeyword;
                summary.HtmlResult = html;
                summary.Url = pageAnalysis.AbsoluteUrl;

                var analyzerResult = new SummaryResult
                {
                    Alias = summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.SummaryResults.Add(analyzerResult);
            }
        }
    }
}