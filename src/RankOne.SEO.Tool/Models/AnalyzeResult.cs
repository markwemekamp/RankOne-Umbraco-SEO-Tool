using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Models
{
    public class AnalyzeResult
    {
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

        public int Weight
        {
            get; set;
        }

        public AnalyzeResult()
        {
            ResultRules = new List<ResultRule>();
            Weight = 100;
        }

        public void AddResultRule(string code, string type)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (type == null) throw new ArgumentNullException(nameof(type));

            ResultRules.Add(new ResultRule { Alias = code, Type = type });
        }

        public int CountRestultRulesByType(string type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return ResultRules.Count(x => x.Type == type);
        }
    }
}