using HtmlAgilityPack;
using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IAnalyzer
    {
        AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url);
    }
}
