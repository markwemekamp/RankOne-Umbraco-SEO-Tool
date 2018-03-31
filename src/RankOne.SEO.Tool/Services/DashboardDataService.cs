using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using Umbraco.Web;

namespace RankOne.Services
{
    public class DashboardDataService : IDashboardDataService
    {
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;
        private readonly IPageScoreNodeHelper _pageScoreNodeHelper;
        private readonly INodeReportRepository _nodeReportRepository;

        public DashboardDataService() : this(RankOneContext.Instance)
        { }

        public DashboardDataService(IRankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.PageScoreNodeHelper.Value,
            rankOneContext.NodeReportRepository.Value)
        { }

        public DashboardDataService(ITypedPublishedContentQuery typedPublishedContentQuery, IPageScoreNodeHelper pageScoreNodeHelper, 
            INodeReportRepository nodeReportRepository)
        {
            if (typedPublishedContentQuery == null) throw new ArgumentNullException(nameof(typedPublishedContentQuery));
            if (pageScoreNodeHelper == null) throw new ArgumentNullException(nameof(pageScoreNodeHelper));
            if (nodeReportRepository == null) throw new ArgumentNullException(nameof(nodeReportRepository));

            _typedPublishedContentQuery = typedPublishedContentQuery;
            _pageScoreNodeHelper = pageScoreNodeHelper;
            _nodeReportRepository = nodeReportRepository;
        }

        public void Initialize()
        {
            _nodeReportRepository.CreateTable();
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