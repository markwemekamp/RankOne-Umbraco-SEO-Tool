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
            get { return CountRestultRulesByType(ResultType.Error); }
        }

        public int WarningCount
        {
            get { return CountRestultRulesByType(ResultType.Warning); }
        }

        public int HintCount
        {
            get { return CountRestultRulesByType(ResultType.Hint); }
        }

        public int SuccessCount
        {
            get { return CountRestultRulesByType(ResultType.Success); }
        }

        public int Score { get; set; }

        public void AddResultRule(string code, string type)
        {
            ResultRules.Add(new ResultRule { Alias = code, Type = type });
        }

        public int CountRestultRulesByType(string type)
        {
            return ResultRules.Count(x => x.Type == type);
        }

    }
}
