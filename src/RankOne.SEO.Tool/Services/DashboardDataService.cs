using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Services
{
    public class DashboardDataService : IDashboardDataService
    {
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;
        private readonly IPageScoreNodeHelper _pageScoreNodeHelper;
        private readonly INodeReportService _nodeReportService;

        public DashboardDataService() : this(RankOneContext.Instance)
        { }

        public DashboardDataService(IRankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.PageScoreNodeHelper.Value, 
            rankOneContext.NodeReportService.Value)
        { }

        public DashboardDataService(ITypedPublishedContentQuery typedPublishedContentQuery, IPageScoreNodeHelper pageScoreNodeHelper, INodeReportService nodeReportService)
        {
            _typedPublishedContentQuery = typedPublishedContentQuery;
            _pageScoreNodeHelper = pageScoreNodeHelper;
            _nodeReportService = nodeReportService;
        }

        public void Initialize()
        {
            _nodeReportService.CreateTable();
        }

        public IEnumerable<PageScoreNode> GetHierarchyFromCache()
        {
            try
            {
                var nodeCollection = _typedPublishedContentQuery.TypedContentAtRoot();
                return _pageScoreNodeHelper.GetPageScoresFromCache(nodeCollection);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<PageScoreNode> GetUpdatedHierarchy()
        {
            try
            {
                var nodeCollection = _typedPublishedContentQuery.TypedContentAtRoot();
                return _pageScoreNodeHelper.UpdatePageScores(nodeCollection);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}