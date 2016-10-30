using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IAnalyzer
    {
        AnalyzeResult Analyse(PageData pageData);
    }
}
