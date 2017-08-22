using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IAnalyzer
    {
        string Alias { get; set; }
        IEnumerable<IOption> Options { get; set; }

        AnalyzeResult Analyse(IPageData pageData);
    }
}