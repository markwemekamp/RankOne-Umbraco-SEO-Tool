using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        public string Alias { get; set; }
        public abstract AnalyzeResult Analyse(IPageData pageData);
    }
}