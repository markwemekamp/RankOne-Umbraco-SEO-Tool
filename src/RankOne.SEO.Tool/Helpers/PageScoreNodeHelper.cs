using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class PageScoreNodeHelper : IPageScoreNodeHelper
    {
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;
        private readonly INodeReportService _nodeReportService;
        private readonly IPageScoreSerializer _pagescoreSerializer;
        private readonly IAnalyzeService _analyzeService;

        public PageScoreNodeHelper() : this(RankOneContext.Instance)
        { }

        public PageScoreNodeHelper(IRankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.NodeReportService.Value,
            rankOneContext.PageScoreSerializer.Value, rankOneContext.AnalyzeService.Value)
        { }

        public PageScoreNodeHelper(ITypedPublishedContentQuery typedPublishedContentQuery, INodeReportService nodeReportService,
            IPageScoreSerializer pageScoreSerializer, IAnalyzeService analyzeService)
        {
            if (typedPublishedContentQuery == null) throw new ArgumentNullException(nameof(typedPublishedContentQuery));
            if (nodeReportService == null) throw new ArgumentNullException(nameof(nodeReportService));
            if (pageScoreSerializer == null) throw new ArgumentNullException(nameof(pageScoreSerializer));
            if (analyzeService == null) throw new ArgumentNullException(nameof(analyzeService));

            _typedPublishedContentQuery = typedPublishedContentQuery;
            _nodeReportService = nodeReportService;
            _pagescoreSerializer = pageScoreSerializer;
            _analyzeService = analyzeService;
        }

        public IEnumerable<PageScoreNode> GetPageScoresFromCache(IEnumerable<IPublishedContent> nodeCollection)
        {
            if (nodeCollection == null) throw new ArgumentNullException(nameof(nodeCollection));

            var nodeHierarchy = GetPageHierarchy(nodeCollection);
            foreach (var node in nodeHierarchy)
            {
                SetPageScore(node);
            }
            return nodeHierarchy;
        }

        public IEnumerable<PageScoreNode> UpdatePageScores(IEnumerable<IPublishedContent> nodeCollection)
        {
            if (nodeCollection == null) throw new ArgumentNullException(nameof(nodeCollection));

            var nodeHierarchy = GetPageHierarchy(nodeCollection);
            foreach (var node in nodeHierarchy)
            {
                UpdatePageScore(node);
            }
            return nodeHierarchy;
        }

        private IEnumerable<PageScoreNode> GetPageHierarchy(IEnumerable<IPublishedContent> nodeCollection)
        {
            var nodeHiearchyCollection = new List<PageScoreNode>();
            foreach (var node in nodeCollection)
            {
                var nodeHierarchy = new PageScoreNode
                {
                    NodeInformation = new NodeInformation(node)
                };
                if (node.Children != null)
                {
                    nodeHierarchy.Children = GetPageHierarchy(node.Children);
                }
                nodeHiearchyCollection.Add(nodeHierarchy);
            }
            return nodeHiearchyCollection;
        }

        private void SetPageScore(PageScoreNode node)
        {
            var nodeReport = _nodeReportService.GetById(node.NodeInformation.Id);
            if (nodeReport != null)
            {
                if (node.NodeInformation.TemplateId == 0)
                {
                    _nodeReportService.Delete(nodeReport);
                }
                if (node.NodeInformation.TemplateId > 0 || node.HasChildrenWithTemplate)
                {
                    node.FocusKeyword = nodeReport.FocusKeyword;
                    try
                    {
                        node.PageScore = _pagescoreSerializer.Deserialize(nodeReport.Report);
                    }
                    catch (Exception)
                    {
                        // delete database copy
                        _nodeReportService.Delete(nodeReport);
                    }
                    foreach (var childNode in node.Children)
                    {
                        SetPageScore(childNode);
                    }
                }
            }
        }

        private void UpdatePageScore(PageScoreNode node)
        {
            if (node.NodeInformation.TemplateId > 0)
            {
                var umbracoNode = node.NodeInformation.Node;
                var analysis = _analyzeService.CreateAnalysis(umbracoNode);

                if (analysis != null)
                {
                    node.FocusKeyword = analysis.FocusKeyword;
                    node.PageScore = analysis.Score;
                }
            }
            foreach (var childNode in node.Children)
            {
                UpdatePageScore(childNode);
            }
        }
    }
}