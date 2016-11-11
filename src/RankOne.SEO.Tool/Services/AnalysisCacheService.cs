using System;
using System.Web.Script.Serialization;
using RankOne.Models;
using RankOne.Repositories;

namespace RankOne.Services
{
    public class AnalysisCacheService
    {
        private readonly NodeReportRepository _nodeReportRepository;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        public AnalysisCacheService()
        {
            _nodeReportRepository = new NodeReportRepository();
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public void SaveCachedAnalysis(int id, string focusKeyword, PageAnalysis pageAnalysis)
        {
            if (_nodeReportRepository.DatabaseExists())
            {
                var scoreReport = _javaScriptSerializer.Serialize(pageAnalysis.Score);

                var nodeReport = new NodeReport
                {
                    Id = id,
                    FocusKeyword = focusKeyword,
                    Report = scoreReport,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                CreateOrUpdateNodeReport(nodeReport);
            }
        }

        private void CreateOrUpdateNodeReport(NodeReport nodeReport)
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
