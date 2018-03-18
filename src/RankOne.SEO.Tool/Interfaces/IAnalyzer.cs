using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IAnalyzer
    {
        string Alias { get; set; }
        IEnumerable<IOption> Options { get; set; }
        AnalyzeResult AnalyzeResult { get; }
    int Weight { get; set; }

        void Analyse(IPageData pageData);
    }
}