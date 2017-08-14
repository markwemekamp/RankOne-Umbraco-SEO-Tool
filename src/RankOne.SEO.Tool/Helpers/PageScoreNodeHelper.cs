using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Repositories;
using RankOne.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class PageScoreNodeHelper : IPageScoreNodeHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly NodeReportRepository _nodeReportRepository;
        private readonly JavaScriptSerializer _javascriptSerializer;
        private readonly AnalyzeService _analyzeService;

        public PageScoreNodeHelper()
        {
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _nodeReportRepository = new NodeReportRepository();
            _javascriptSerializer = new JavaScriptSerializer();
            _analyzeService = new AnalyzeService();
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
                        node.PageScore = _javascriptSerializer.Deserialize<PageScore>(nodeReport.Report);
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
                var umbracoNode = _umbracoHelper.TypedContent(node.NodeInformation.Id);
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