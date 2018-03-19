using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        public string Alias { get; set; }
        public int Weight { get; set; }

        public IEnumerable<IOption> Options { get; set; }
        public AnalyzeResult AnalyzeResult { get; internal set; }

        public BaseAnalyzer()
        {
            AnalyzeResult = new AnalyzeResult() { Weight = Weight, Alias = Alias };
        }

        public void AddResultRule(string code, string type)
        {
            AnalyzeResult.AddResultRule(code, type);
        }

        public void AddResultRule(ResultRule resultRule)
        {
            AnalyzeResult.ResultRules.Add(resultRule);
        }

        public abstract void Analyse(IPageData pageData);
    }
}