using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Repositories
{
    public class NodeReportRepository : BaseRepository<NodeReport>, INodeReportRepository
    {
        public NodeReportRepository() : this(RankOneContext.Instance)
        { }

        public NodeReportRepository(RankOneContext rankOneContext) : base(rankOneContext.DatabaseContext.Value)
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