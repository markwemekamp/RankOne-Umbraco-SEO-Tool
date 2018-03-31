using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Services
{
    public class AnalysisCacheService : IAnalysisCacheService
    {
        private readonly INodeReportRepository _nodeReportRepository;
        private readonly IPageScoreSerializer _pageScoreSerializer;

        public AnalysisCacheService() : this(RankOneContext.Instance)
        { }

        public AnalysisCacheService(IRankOneContext rankOneContext) : this(rankOneContext.NodeReportRepository.Value, rankOneContext.PageScoreSerializer.Value)
        { }

        public AnalysisCacheService(INodeReportRepository nodeReportService, IPageScoreSerializer pageScoreSerializer)
        {
            if (nodeReportService == null) throw new ArgumentNullException(nameof(nodeReportService));
            if (pageScoreSerializer == null) throw new ArgumentNullException(nameof(pageScoreSerializer));

            _nodeReportRepository = nodeReportService;
            _pageScoreSerializer = pageScoreSerializer;
        }

        public void Save(int id, PageAnalysis pageAnalysis)
        {
            if (id < 0) throw new ArgumentException(nameof(id));
            if (pageAnalysis == null) throw new ArgumentNullException(nameof(pageAnalysis));

            if (_nodeReportRepository.TableExists)
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

                Save(nodeReport);
            }
        }

        private void Save(NodeReport nodeReport)
        {
            var dbNodeReport = _nodeReportRepository.GetById(nodeReport.Id);
            if (dbNodeReport == null)
            {
                _nodeReportRepository.Insert(nodeReport);
            }
            else
            {
                dbNodeReport.FocusKeyword = nodeReport.FocusKeyword;
                dbNodeReport.Report = nodeReport.Report;
                dbNodeReport.UpdatedOn = nodeReport.UpdatedOn;

                _nodeReportRepository.Update(nodeReport);
            }
        }
    }
}