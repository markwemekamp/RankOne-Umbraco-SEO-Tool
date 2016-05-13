using System.Collections.Generic;
using System.Net;

namespace RankOne.Business.Models
{
    public class PageAnalysisVm
    {
        public string Url { get; set; }
        public HtmlResultVm HtmlResult { get; set; }
        public List<AnalyzerResult> AnalyzerResults { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
