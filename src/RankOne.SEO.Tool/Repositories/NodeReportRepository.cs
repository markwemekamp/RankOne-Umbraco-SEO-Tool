using RankOne.Models;
using System;
using Umbraco.Web;

namespace RankOne.Repositories
{
    public class NodeReportRepository : BaseRepository<NodeReport>
    {
        public NodeReportRepository() : base(UmbracoContext.Current.Application.DatabaseContext)
        { }

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