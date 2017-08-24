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

        public abstract AnalyzeResult Analyse(IPageData pageData);
    }
}