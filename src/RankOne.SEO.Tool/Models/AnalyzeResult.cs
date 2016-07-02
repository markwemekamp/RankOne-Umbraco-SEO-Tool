using System.Collections.Generic;
using System.Linq;

namespace RankOne.Models
{
    public class AnalyzeResult
    {
        public AnalyzeResult()
        {
            ResultRules = new List<ResultRule>();
        }

        public string Alias { get; set; }
        public List<ResultRule> ResultRules { get; set; }

        public int ErrorCount
        {
            get { return ResultRules.Count(x => x.Type == ResultType.Error); }
        }

        public int WarningCount
        {
            get { return ResultRules.Count(x => x.Type == ResultType.Warning); }
        }

        public int HintCount
        {
            get { return ResultRules.Count(x => x.Type == ResultType.Hint); }
        }

        public int SuccessCount
        {
            get { return ResultRules.Count(x => x.Type == ResultType.Success); }
        }

        public int Score { get; set; }

        public void AddResultRule(string code, string type)
        {
            ResultRules.Add(new ResultRule { Alias = code, Type = type });
        }
    }
}
