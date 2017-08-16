using System.Linq;

namespace RankOne.Models
{
    public class AnalyzerResult
    {
        public string Alias { get; set; }
        public Analysis Analysis { get; set; }

        public int ErrorCount
        {
            get { return Analysis.Results.Sum(x => x.ErrorCount); }
        }

        public int WarningCount
        {
            get { return Analysis.Results.Sum(x => x.WarningCount); }
        }

        public int HintCount
        {
            get { return Analysis.Results.Sum(x => x.HintCount); }
        }

        public int SuccessCount
        {
            get { return Analysis.Results.Sum(x => x.SuccessCount); }
        }
    }
}