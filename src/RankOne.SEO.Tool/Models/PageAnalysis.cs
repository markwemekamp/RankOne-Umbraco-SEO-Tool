using System.Collections.Generic;
using System.Net;

namespace RankOne.Models
{
    public class PageAnalysis
    {
        public string AbsoluteUrl { get; set; }
        public string FocusKeyword { get; set; }
        public int Size { get; set; }
        public List<SummaryResult> SummaryResults { get; set; }
        public HttpStatusCode Status { get; set; }
        public PageScore Score { get; set; }

        public PageAnalysis()
        {
            Status = HttpStatusCode.OK;
            SummaryResults = new List<SummaryResult>();
        }
    }
}