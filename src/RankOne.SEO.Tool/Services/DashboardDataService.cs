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
        private readonly DatabaseContext _databaseContext;

        public DashboardDataService() : this(RankOneContext.Instance)
        { }

        public DashboardDataService(RankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.PageScoreNodeHelper.Value, rankOneContext.DatabaseContext.Value)
        { }

        public DashboardDataService(ITypedPublishedContentQuery typedPublishedContentQuery, IPageScoreNodeHelper pageScoreNodeHelper, DatabaseContext databaseContext)
        {
            _typedPublishedContentQuery = typedPublishedContentQuery;
            _pageScoreNodeHelper = pageScoreNodeHelper;
            _databaseContext = databaseContext;
        }

        public void Initialize()
        {
            var databaseSchemaHelper = new DatabaseSchemaHelper(_databaseContext.Database, LoggerResolver.Current.Logger, _databaseContext.SqlSyntax);
            databaseSchemaHelper.CreateTable<NodeReport>(false);
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