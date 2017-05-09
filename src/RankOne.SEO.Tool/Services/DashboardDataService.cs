using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Services
{
    public class DashboardDataService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IPageScoreNodeHelper _pageScoreNodeHelper;
        private readonly TableNameHelper<NodeReport> _tableNameHelper;
        private readonly DatabaseSchemaHelper _databaseSchemaHelper;

        public DashboardDataService()
        {
            _pageScoreNodeHelper = new PageScoreNodeHelper();
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _tableNameHelper = new TableNameHelper<NodeReport>();
            var databaseContext = UmbracoContext.Current.Application.DatabaseContext;
            _databaseSchemaHelper = new DatabaseSchemaHelper(databaseContext.Database, LoggerResolver.Current.Logger, databaseContext.SqlSyntax);
        }

        public void Initialize()
        {
            var tableName = _tableNameHelper.GetTableName();

            if (!_databaseSchemaHelper.TableExist(tableName))
            {
                _databaseSchemaHelper.CreateTable<NodeReport>(false);
            }
        }

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> the score from the database is used, else it will be calculated.</param>
        /// <returns></returns>
        public List<PageScoreNode> GetHierarchy(bool useCache = true)
        {
            var tableName = _tableNameHelper.GetTableName();

            if (_databaseSchemaHelper.TableExist(tableName))
            {
                var nodeCollection = _umbracoHelper.TypedContentAtRoot();
                return _pageScoreNodeHelper.GetPageHierarchy(nodeCollection, useCache);
            }
            return null;
        }
    }
}
