using HtmlAgilityPack;
using RankOne.Business.Models;
using RankOne.Business.Interfaces;

namespace RankOne.Business.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        protected HtmlHelper HtmlHelper;

        protected BaseAnalyzer()
        {
            HtmlHelper = new HtmlHelper();
        }

        public abstract AnalyzeResult Analyse(HtmlNode document);
    }
}
