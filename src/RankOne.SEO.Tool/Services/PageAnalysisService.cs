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
        private readonly INodeReportRepository _nodeReportRepository;
        private readonly IPageScoreSerializer _pageScoreSerializer;

        public PageAnalysisService() : this(RankOneContext.Instance)
        { }

        public PageAnalysisService(IRankOneContext rankOneContext) : this(rankOneContext.ScoreService.Value, rankOneContext.ByteSizeHelper.Value,
            rankOneContext.Summaries.Value, rankOneContext.TemplateHelper.Value, rankOneContext.NodeReportRepository.Value, rankOneContext.PageScoreSerializer.Value)
        { }

        public PageAnalysisService(IScoreService scoreService, IByteSizeHelper byteSizeHelper, IEnumerable<ISummary> summaries,
            ITemplateHelper templateHelper, INodeReportRepository nodeReportService, IPageScoreSerializer pageScoreSerializer)
        {
            if (scoreService == null) throw new ArgumentNullException(nameof(scoreService));
            if (byteSizeHelper == null) throw new ArgumentNullException(nameof(byteSizeHelper));
            if (summaries == null) throw new ArgumentNullException(nameof(summaries));
            if (templateHelper == null) throw new ArgumentNullException(nameof(templateHelper));
            if (nodeReportService == null) throw new ArgumentNullException(nameof(nodeReportService));
            if (pageScoreSerializer == null) throw new ArgumentNullException(nameof(pageScoreSerializer));

            _scoreService = scoreService;
            _byteSizeHelper = byteSizeHelper;
            _summaries = summaries;
            _templateHelper = templateHelper;
            _nodeReportRepository = nodeReportService;
            _pageScoreSerializer = pageScoreSerializer;
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

        public void Save(int id, PageAnalysis pageAnalysis)
        {
            if (id < 0) throw new ArgumentException(nameof(id));
            if (pageAnalysis == null) throw new ArgumentNullException(nameof(pageAnalysis));

            if (_nodeReportRepository.TableExists)
            {
                var scoreReport = _pageScoreSerializer.Serialize(pageAnalysis.Score);

                var nodeReport = new NodeReport
                {
                    Id = id,
                    FocusKeyword = pageAnalysis.FocusKeyword,
                    Report = scoreReport,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                Save(nodeReport);
            }
        }

        private void Save(NodeReport nodeReport)
        {
            var dbNodeReport = _nodeReportRepository.GetById(nodeReport.Id);
            if (dbNodeReport == null)
            {
                _nodeReportRepository.Insert(nodeReport);
            }
            else
            {
                dbNodeReport.FocusKeyword = nodeReport.FocusKeyword;
                dbNodeReport.Report = nodeReport.Report;
                dbNodeReport.UpdatedOn = nodeReport.UpdatedOn;

                _nodeReportRepository.Update(nodeReport);
            }
        }
    }
}