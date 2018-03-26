using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Repositories
{
    public class AnalysisCacheRepository : IAnalysisCacheRepository
    {
        private readonly INodeReportService _nodeReportService;
        private readonly IPageScoreSerializer _pageScoreSerializer;

        public AnalysisCacheRepository() : this(RankOneContext.Instance)
        { }

        public AnalysisCacheRepository(IRankOneContext rankOneContext) : this(rankOneContext.NodeReportService.Value, rankOneContext.PageScoreSerializer.Value)
        { }

        public AnalysisCacheRepository(INodeReportService nodeReportService, IPageScoreSerializer pageScoreSerializer)
        {
            if (nodeReportService == null) throw new ArgumentNullException(nameof(nodeReportService));
            if (pageScoreSerializer == null) throw new ArgumentNullException(nameof(pageScoreSerializer));

            _nodeReportService = nodeReportService;
            _pageScoreSerializer = pageScoreSerializer;
        }

        public void Save(int id, PageAnalysis pageAnalysis)
        {
            if (id < 0) throw new ArgumentException(nameof(id));
            if (pageAnalysis == null) throw new ArgumentNullException(nameof(pageAnalysis));

            if (_nodeReportService.TableExists)
            {
                var scoreReport = _pageScoreSerializer.Serialize(pageAnalysis.Score);

                var nodeReport = new NodeReport
                {
                    Id = id,
                    FocusKeyword = pageAnalysis.FocusKeyword,
                    Report = scoreReport,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                CreateOrUpdateNodeReport(nodeReport);
            }
        }

        private void CreateOrUpdateNodeReport(NodeReport nodeReport)
        {
            var dbNodeReport = _nodeReportService.GetById(nodeReport.Id);
            if (dbNodeReport == null)
            {
                _nodeReportService.Insert(nodeReport);
            }
            else
            {
                dbNodeReport.FocusKeyword = nodeReport.FocusKeyword;
                dbNodeReport.Report = nodeReport.Report;
                dbNodeReport.UpdatedOn = nodeReport.UpdatedOn;

                _nodeReportService.Update(nodeReport);
            }
        }
    }
}