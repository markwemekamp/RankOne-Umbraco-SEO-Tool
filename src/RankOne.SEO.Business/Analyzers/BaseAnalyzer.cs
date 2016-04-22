using HtmlAgilityPack;
using RankOne.Business.Models;
using RankOne.Business.Interfaces;
using Umbraco.Web.Media.EmbedProviders.Settings;

namespace RankOne.Business.Analyzers
{
    public abstract class BaseAnalyzer : IAnalyzer
    {
        protected HtmlHelper HtmlHelper;

        protected BaseAnalyzer()
        {
            HtmlHelper = new HtmlHelper();
        }

        public abstract AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues);
    }
}
