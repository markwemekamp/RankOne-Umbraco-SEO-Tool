using System.Collections.Generic;
using System.Net;

namespace RankOne.Business.Models
{
    public class PageAnalysis
    {
        public HtmlResult HtmlResult { get; set; }
        public List<AnalyzerResult> AnalyzerResults { get; set; }
        public HttpStatusCode Status { get; set; }

        public PageAnalysis()
        {
            Status = HttpStatusCode.OK;
            AnalyzerResults = new List<AnalyzerResult>();
        }
    }
}
