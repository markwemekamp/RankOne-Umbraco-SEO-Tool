using System.Collections.Generic;

namespace RankOne.Business.Models
{
    public class AnalysisInformation
    {
        public string Code { get; set; }

        public List<string> Tokens { get; set; }

        public AnalysisInformation()
        {
            Tokens = new List<string>();
        }
    }
}
