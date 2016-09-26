using System.Web.Script.Serialization;
using RankOne.Models;
using RankOne.Repositories;

namespace RankOne.Services
{
    public class NodeReportService
    {
        private readonly NodeReportRepository _nodeReportRepository;

        public NodeReportService()
        {
            _nodeReportRepository = new NodeReportRepository();
        }

        public void Save(int id, string focusKeyword, PageAnalysis pageAnalysis)
        {
            if (_nodeReportRepository.DatabaseExists())
            {
                var serializer = new JavaScriptSerializer();

                var json = serializer.Serialize(pageAnalysis.Score);

                var nodeReport = _nodeReportRepository.GetById(id);
                if (nodeReport == null)
                {
                    nodeReport = new NodeReport
                    {
                        Id = id,
                        FocusKeyword = focusKeyword,
                        Report = json
                    };

                    _nodeReportRepository.Insert(nodeReport);
                }
                else
                {
                    nodeReport.FocusKeyword = focusKeyword;
                    nodeReport.Report = json;

                    _nodeReportRepository.Update(nodeReport);
                }
            }
        }

    }
}
