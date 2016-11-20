using System;
using System.Web.Script.Serialization;
using RankOne.Models;

namespace RankOne.Repositories
{
    public class AnalysisCacheRepository
    {
        private readonly NodeReportRepository _nodeReportRepository;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        public AnalysisCacheRepository()
        {
            _nodeReportRepository = new NodeReportRepository();
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public void Save(int id, PageAnalysis pageAnalysis)
        {
            var scoreReport = _javaScriptSerializer.Serialize(pageAnalysis.Score);

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
