using System.Xml.Linq;
using RankOne.Business.Models;

namespace RankOne.Business.Interfaces
{
    public interface IAnalyzer
    {
        AnalyzeResult Analyse(XDocument document);
    }
}
