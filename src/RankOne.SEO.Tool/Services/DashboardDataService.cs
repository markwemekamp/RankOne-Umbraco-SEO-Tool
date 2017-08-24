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

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> the score from the database is used, else it will be calculated.</param>
        /// <returns></returns>
        public IEnumerable<PageScoreNode> GetHierarchy(bool useCache = true)
        {
            try
            {
                var nodeCollection = _typedPublishedContentQuery.TypedContentAtRoot();
                return _pageScoreNodeHelper.GetPageHierarchy(nodeCollection, useCache);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}