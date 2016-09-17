using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using RankOne.Models;
using RankOne.Repositories;
using RankOne.Services;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    public class RankOneApiController : UmbracoAuthorizedApiController
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly NodeReportRepository _nodeReportRepository;
        private readonly JavaScriptSerializer _javascriptSerializer;
        private readonly AnalyzeService _analyzeService;

        public RankOneApiController()
        {
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _nodeReportRepository = new NodeReportRepository();
            _javascriptSerializer = new JavaScriptSerializer();
            _analyzeService = new AnalyzeService();
        }

        /// <summary>
        /// Analyzes the node.
        /// The focus keyword can be given as a parameter here for performance reasons and the check the node
        /// against specified keywords.
        /// If the keyword option is set to null, the AnalyzeService tries to determine if the keyword is set
        /// by analyzing the node's properties.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="focusKeyword">The focus keyword.</param>
        /// <returns></returns>
        [HttpGet]
        public PageAnalysis AnalyzeNode(int id, string focusKeyword = null)
        {
            try
            {
                return  _analyzeService.AnalyzeWebPage(id, focusKeyword);
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(RankOneApiController), "RankOne AnalyzeNode Exception", ex);
                throw;
            }
        }

        [HttpGet]
        public PageInformation GetPageInformation(int id)
        {
            try
            {
                var pageInformationService = new PageInformationService();
                var result = pageInformationService.GetpageInformation(id);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(RankOneApiController), "RankOne GetPageInformation Exception", ex);
                throw;
            }
        }

        [HttpGet]
        public IEnumerable<HiearchyNode> GetPageHierarchy()
        {
            if (_nodeReportRepository.DatabaseExists())
            {
                var nodeCollection = _umbracoHelper.TypedContentAtRoot();
                var nodeHierarchy = GetHierarchy(nodeCollection);

                GetPageScores(nodeHierarchy);

                return nodeHierarchy;
            }
            return null;
        }

        [HttpGet]
        public IEnumerable<HiearchyNode> Initialize()
        {
            if (!_nodeReportRepository.DatabaseExists())
            {
                _nodeReportRepository.CreateTable();
            }

            return UpdateAllPages();
        }

        [HttpGet]
        public IEnumerable<HiearchyNode> UpdateAllPages()
        {
            var nodeCollection = _umbracoHelper.TypedContentAtRoot();
            var nodeHierarchy = GetHierarchy(nodeCollection);

            foreach (var node in nodeHierarchy)
            {
                UpdatePageScore(node);
            }

            return nodeHierarchy;
        }

        private void GetPageScores(IEnumerable<HiearchyNode> nodeHierarchy)
        {
            foreach (var node in nodeHierarchy)
            {
                if (node.NodeInformation.TemplateId > 0)
                {
                    var nodeReport = _nodeReportRepository.GetById(node.NodeInformation.Id);
                    if (nodeReport != null)
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
                    GetPageScores(node.Children);
                }
            }
        }

        private void UpdatePageScore(HiearchyNode node)
        {
            if (node.NodeInformation.TemplateId > 0)
            {
                var analysis = _analyzeService.AnalyzeWebPage(node.NodeInformation.Id);

                node.FocusKeyword = analysis.FocusKeyword;
                node.PageScore = analysis.Score;

                foreach (var childNode in node.Children)
                {
                    UpdatePageScore(childNode);
                }
            }
        }

        private List<HiearchyNode> GetHierarchy(IEnumerable<IPublishedContent> nodeCollection)
        {
            var nodeHiearchyCollection = new List<HiearchyNode>();
            foreach (var node in nodeCollection)
            {
                var nodeHierarchy = new HiearchyNode
                {
                    NodeInformation = new NodeInformation
                    {
                        Id = node.Id,
                        Name = node.Name,
                        TemplateId = node.TemplateId
                    },
                    Children = GetHierarchy(node.Children)
                };

                nodeHiearchyCollection.Add(nodeHierarchy);
            }
            return nodeHiearchyCollection;
        }
    }
}
