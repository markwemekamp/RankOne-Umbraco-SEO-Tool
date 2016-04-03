using System.Collections.Generic;

namespace RankOne.Business.Models
{
    public class Analysis
    {
        public Analysis()
        {
            Results = new List<AnalyzeResult>();
            Information = new List<AnalysisInformation>();
        }

        public List<AnalyzeResult> Results { get; set; }

        public List<AnalysisInformation> Information { get; set; } 
    }
}
