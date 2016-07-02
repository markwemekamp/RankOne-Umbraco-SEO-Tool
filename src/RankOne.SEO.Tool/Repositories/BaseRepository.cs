using System.Collections.Generic;
using System.Linq;
using RankOne.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace RankOne.Repositories
{
    public class BaseRepository<T> where T : BaseDatabaseObject
    {
        protected DatabaseContext DatabaseContext;
        protected UmbracoDatabase Database;
        protected DatabaseSchemaHelper DatabaseSchemaHelper;

        public BaseRepository(DatabaseContext context)
        {
            DatabaseContext = context;
            Database = context.Database;
            DatabaseSchemaHelper = new DatabaseSchemaHelper(Database, LoggerResolver.Current.Logger, DatabaseContext.SqlSyntax);
        }

        public virtual T GetById(int id, string table)
        {
            var query = new Sql().Select("*").From(table).Where("Id = @0", id);
            return Database.Fetch<T>(query).FirstOrDefault();
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
