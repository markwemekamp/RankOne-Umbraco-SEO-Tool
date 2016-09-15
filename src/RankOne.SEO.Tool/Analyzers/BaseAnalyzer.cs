using HtmlAgilityPack;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        protected HtmlHelper HtmlHelper;

        protected BaseAnalyzer()
        {
            HtmlHelper = new HtmlHelper();
        }

        public abstract AnalyzeResult Analyse(HtmlNode document, string focuskeyword, string url);
    }
}
