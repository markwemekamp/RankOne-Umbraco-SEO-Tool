using System.Collections.Generic;

namespace RankOne.Business.Models
{
    public class AnalyzeResult
    {
        public AnalyzeResult()
        {
            ResultRules = new List<ResultRule>();
        }

        public string Alias { get; set; }
        public List<ResultRule> ResultRules { get; set; }

        public void AddResultRule(string code, string type)
        {
            ResultRules.Add(new ResultRule { Alias = code, Type = type });
        }
    }
}
