using System;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Repositories
{
    public class SchemaRepository<T> where T : BaseDatabaseObject
    {
        private string _tableName;
        protected DatabaseSchemaHelper DatabaseSchemaHelper;
        private ITableNameHelper<T> _tableNameHelper;

        public ITableNameHelper<T> TableNameHelper
        {
            get
            {
                if (_tableNameHelper == null)
                {
                    _tableNameHelper = new TableNameHelper<T>();
                }
                return _tableNameHelper;
            }
            set { _tableNameHelper = value; }
        }

        public string TableName
        {
            get
            {
                if (_tableName == null)
                {
                    _tableName = TableNameHelper.GetTableName();
                }
                return _tableName;
            }
        }

        public SchemaRepository() : this(UmbracoContext.Current.Application.DatabaseContext)
        { }

        public SchemaRepository(DatabaseContext context) : this(context, LoggerResolver.Current.Logger)
        { }


        public SchemaRepository(DatabaseContext context, ILogger logger)
        {
            DatabaseSchemaHelper = new DatabaseSchemaHelper(context.Database, logger, context.SqlSyntax);
        }

        public bool DatabaseExists()
        {
            return DatabaseSchemaHelper.TableExist(TableName);
        }

        public void CreateTable()
        {
            DatabaseSchemaHelper.CreateTable<NodeReport>(false);
        }

        public void DeleteTable()
        {
            DatabaseSchemaHelper.DropTable<NodeReport>();
        }
    }
}
