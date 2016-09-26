using System;
using System.Collections.Generic;
using RankOne.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RankOne.Repositories
{
    public class NodeReportRepository : BaseRepository<NodeReport>
    {
        private readonly string TableName = "NodeReport";

        public NodeReportRepository() : base(UmbracoContext.Current.Application.DatabaseContext)
        {}

        public bool DatabaseExists()
        {
            return DatabaseSchemaHelper.TableExist(TableName);
        }

        public void CreateTable()
        {
            DatabaseSchemaHelper.CreateTable<NodeReport>(false);
        }

        public NodeReport GetById(int id)
        {
            return GetById(id, TableName);
        }

        public IEnumerable<NodeReport> GetAll()
        {
            var query = new Sql().Select("*").From(TableName);
            return GetAllByQuery(query);
        }

        public override NodeReport Insert(NodeReport dbEntity)
        {
            dbEntity.CreatedOn = DateTime.Now;
            return base.Insert(dbEntity);
        }

        public override NodeReport Update(NodeReport dbEntity)
        {
            dbEntity.UpdatedOn = DateTime.Now;
            return base.Update(dbEntity);
        }
    }
}
