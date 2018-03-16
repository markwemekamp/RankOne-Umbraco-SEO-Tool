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
        private readonly INodeReportRepository _nodeReportRepository;
        private readonly IPageScoreSerializer _pagescoreSerializer;
        private readonly IAnalyzeService _analyzeService;

        public PageScoreNodeHelper() : this(RankOneContext.Instance)
        { }

        public PageScoreNodeHelper(RankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.NodeReportRepository.Value, rankOneContext.PageScoreSerializer.Value, rankOneContext.AnalyzeService.Value)
        { }

        public PageScoreNodeHelper(ITypedPublishedContentQuery typedPublishedContentQuery, INodeReportRepository nodeReportRepository, IPageScoreSerializer pageScoreSerializer, IAnalyzeService analyzeService)
        {
            _typedPublishedContentQuery = typedPublishedContentQuery;
            _nodeReportRepository = nodeReportRepository;
            _pagescoreSerializer = pageScoreSerializer;
            _analyzeService = analyzeService;
        }

        public IEnumerable<PageScoreNode> GetPageScoresFromCache(IEnumerable<IPublishedContent> nodeCollection)
        {
            var nodeHierarchy = GetPageHierarchy(nodeCollection);
            foreach (var node in nodeHierarchy)
            {
                SetPageScore(node);
            }
            return nodeHierarchy;
        }

        public IEnumerable<PageScoreNode> UpdatePageScores(IEnumerable<IPublishedContent> nodeCollection)
        {
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
                    NodeInformation = new NodeInformation(node),
                    Children = GetPageHierarchy(node.Children)
                };
                nodeHiearchyCollection.Add(nodeHierarchy);
            }
            return nodeHiearchyCollection;
        }

        private void SetPageScore(PageScoreNode node)
        {
            var nodeReport = _nodeReportRepository.GetById(node.NodeInformation.Id);
            if (nodeReport != null)
            {
                if (node.NodeInformation.TemplateId == 0)
                {
                    _nodeReportRepository.Delete(nodeReport);
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
                        _nodeReportRepository.Delete(nodeReport);
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

                node.FocusKeyword = analysis.FocusKeyword;
                node.PageScore = analysis.Score;
            }
            foreach (var childNode in node.Children)
            {
                UpdatePageScore(childNode);
            }
        }
    }
}