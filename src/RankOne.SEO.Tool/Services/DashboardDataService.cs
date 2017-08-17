using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Services
{
    public class DashboardDataService : IDashboardDataService
    {
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;
        private readonly IPageScoreNodeHelper _pageScoreNodeHelper;
        private readonly INodeReportTableHelper _nodeReportHelper;

        public DashboardDataService() : this(RankOneContext.Instance)
        { }

        public DashboardDataService(RankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.PageScoreNodeHelper.Value, rankOneContext.NodeReportTableHelper.Value)
        { }

        public DashboardDataService(ITypedPublishedContentQuery typedPublishedContentQuery, IPageScoreNodeHelper pageScoreNodeHelper, INodeReportTableHelper nodeReportHelper)
        {
            _typedPublishedContentQuery = typedPublishedContentQuery;
            _pageScoreNodeHelper = pageScoreNodeHelper;
            _nodeReportHelper = nodeReportHelper;
        }

        public void Initialize()
        {
            var tableName = _nodeReportHelper.GetTableName();

            var databaseContext = UmbracoContext.Current.Application.DatabaseContext;
            var databaseSchemaHelper = new DatabaseSchemaHelper(databaseContext.Database, LoggerResolver.Current.Logger, databaseContext.SqlSyntax);

            if (!databaseSchemaHelper.TableExist(tableName))
            {
                databaseSchemaHelper.CreateTable<NodeReport>(false);
            }
        }

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> the score from the database is used, else it will be calculated.</param>
        /// <returns></returns>
        public IEnumerable<PageScoreNode> GetHierarchy(bool useCache = true)
        {
            var tableName = _nodeReportHelper.GetTableName();

            var databaseContext = UmbracoContext.Current.Application.DatabaseContext;
            var databaseSchemaHelper = new DatabaseSchemaHelper(databaseContext.Database, LoggerResolver.Current.Logger, databaseContext.SqlSyntax);

            if (databaseSchemaHelper.TableExist(tableName))
            {
                var nodeCollection = _typedPublishedContentQuery.TypedContentAtRoot();
                return _pageScoreNodeHelper.GetPageHierarchy(nodeCollection, useCache);
            }
            return null;
        }
    }
}