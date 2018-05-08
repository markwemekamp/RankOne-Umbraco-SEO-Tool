using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        private string _alias;
        private int _weight;

        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
                AnalyzeResult.Alias = _alias;
            }
        }

        public int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                AnalyzeResult.Weight = _weight;
            }
        }

        public IEnumerable<IOption> Options { get; set; }
        public AnalyzeResult AnalyzeResult { get; internal set; }

        public BaseAnalyzer()
        {
            AnalyzeResult = new AnalyzeResult();
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