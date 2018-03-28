using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Services
{
    public class NodeReportService : UmbracoDatabaseService<NodeReport>, INodeReportService
    {
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