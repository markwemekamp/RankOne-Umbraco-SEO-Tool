using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<PageScoreNode> GetPageHierarchy(IEnumerable<IPublishedContent> nodeCollection, bool useCache)
        {
            var nodeHiearchyCollection = new List<PageScoreNode>();
            foreach (var node in nodeCollection)
            {
                var nodeHierarchy = new PageScoreNode
                {
                    NodeInformation = new NodeInformation
                    {
                        Id = node.Id,
                        Name = node.Name,
                        TemplateId = node.TemplateId
                    },
                    Children = GetPageHierarchy(node.Children, useCache)
                };

                if (useCache)
                {
                    SetPageScore(nodeHierarchy);
                }
                else
                {
                    UpdatePageScore(nodeHierarchy);
                }

                nodeHiearchyCollection.Add(nodeHierarchy);
            }

            return nodeHiearchyCollection.ToList();
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
                }
            }
        }

        private void UpdatePageScore(PageScoreNode node)
        {
            if (node.NodeInformation.TemplateId > 0)
            {
                var umbracoNode = _typedPublishedContentQuery.TypedContent(node.NodeInformation.Id);
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