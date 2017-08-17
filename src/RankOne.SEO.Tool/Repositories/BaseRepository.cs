using RankOne.Helpers;
using RankOne.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RankOne.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private string _tableName;
        private ITableNameHelper<T> _tableNameHelper;
        protected DatabaseContext DatabaseContext;
        protected UmbracoDatabase Database;

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

        public BaseRepository(DatabaseContext context)
        {
            DatabaseContext = context;
            Database = context.Database;
        }

        public virtual T GetById(int id)
        {
            var query = new Sql().Select("*").From(TableName).Where("Id = @0", id);
            return Database.Fetch<T>(query).FirstOrDefault();
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
    }
}