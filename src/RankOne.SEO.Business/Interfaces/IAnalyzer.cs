using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Interfaces
{
    public interface IAnalyzer
    {
        AnalyzeResult Analyse(HtmlNode document);
    }
}
