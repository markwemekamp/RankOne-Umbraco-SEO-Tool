using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IAnalyzer
    {
        string Alias { get; set; }
        AnalyzeResult Analyse(IPageData pageData);
    }
}
