using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Repositories
{
    public class UmbracoDatabaseRepository<T> : IDatabaseRepository<T>
    {
        protected DatabaseContext DatabaseContext;
        protected UmbracoDatabase Database;
        private ITableNameHelper<T> _tableNameHelper;
        private DatabaseSchemaHelper _databaseSchemaHelper;
        private string _tableName;
        private bool? _tableExists;

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

        public bool TableExists
        {
            get
            {
                if (!_tableExists.HasValue)
                {
                    var databaseSchemaHelper = new DatabaseSchemaHelper(Database, LoggerResolver.Current.Logger, DatabaseContext.SqlSyntax);
                    _tableExists = databaseSchemaHelper.TableExist(TableName);
                }
                return _tableExists.Value;
            }
        }

        public UmbracoDatabaseRepository() : this(RankOneContext.Instance)
        { }

        public UmbracoDatabaseRepository(RankOneContext instance) : this(instance.UmbracoContext.Value)
        { }

        public UmbracoDatabaseRepository(UmbracoContext umbracoContext)
        {
            if (umbracoContext == null) throw new ArgumentNullException(nameof(umbracoContext));

            DatabaseContext = umbracoContext.Application.DatabaseContext;
            Database = DatabaseContext.Database;
            _databaseSchemaHelper = new DatabaseSchemaHelper(Database, LoggerResolver.Current.Logger, DatabaseContext.SqlSyntax);
        }

        public virtual T GetById(int id)
        {
            var query = new Sql().Select("*").From(TableName).Where("Id = @0", id);
            return GetAllByQuery(query).FirstOrDefault();
        }

        public virtual IEnumerable<T> GetAll()
        {
            var query = new Sql().Select("*").From(TableName);
            return GetAllByQuery(query);
        }

        public virtual IEnumerable<T> GetAllByQuery(Sql query)
        {
            return Database.Fetch<T>(query);
        }

        public virtual T Insert(T dbEntity)
        {
            Database.Insert(dbEntity);
            return dbEntity;
        }

        public virtual T Update(T dbEntity)
        {
            Database.Update(dbEntity);
            return dbEntity;
        }

        public virtual void Delete(T dbEntity)
        {
            Database.Delete(dbEntity);
        }

        public void CreateTable()
        {
            _databaseSchemaHelper.CreateTable(false, typeof(T));
        }
    }
}