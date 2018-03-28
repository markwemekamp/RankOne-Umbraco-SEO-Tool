using HtmlAgilityPack;
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
        private readonly IByteSizeHelper _byteSizeHelper;
        private readonly ITemplateHelper _templateHelper;

        public PageAnalysisService() : this(RankOneContext.Instance)
        { }

        public PageAnalysisService(IRankOneContext rankOneContext) : this(rankOneContext.ScoreService.Value, rankOneContext.ByteSizeHelper.Value,
            rankOneContext.Summaries.Value, rankOneContext.TemplateHelper.Value)
        { }

        public PageAnalysisService(IScoreService scoreService, IByteSizeHelper byteSizeHelper, IEnumerable<ISummary> summaries,
            ITemplateHelper templateHelper)
        {
            if (scoreService == null) throw new ArgumentNullException(nameof(scoreService));
            if (byteSizeHelper == null) throw new ArgumentNullException(nameof(byteSizeHelper));
            if (summaries == null) throw new ArgumentNullException(nameof(summaries));
            if (templateHelper == null) throw new ArgumentNullException(nameof(templateHelper));

            _scoreService = scoreService;
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

                pageAnalysis.AbsoluteUrl = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = _byteSizeHelper.GetByteSize(htmlString);

                SetAnalyzerResults(pageAnalysis, htmlString);
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            return pageAnalysis;
        }

        private void SetAnalyzerResults(PageAnalysis pageAnalysis, string html)
        {
            var htmlNode = CreateHtmlNode(html);

            // Instantiate the types and retrieve te results
            foreach (var summary in _summaries)
            {
                summary.FocusKeyword = pageAnalysis.FocusKeyword;
                summary.Document = htmlNode;
                summary.Url = pageAnalysis.AbsoluteUrl;

                var analyzerResult = new SummaryResult
                {
                    Alias = summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.SummaryResults.Add(analyzerResult);
            }
        }

        private HtmlNode CreateHtmlNode(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode;
        }
    }
}