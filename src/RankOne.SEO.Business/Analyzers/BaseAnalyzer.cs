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

        public string Alias { get; set; }

        public string TitleTag
        {
            get
            {
                return string.Format("{0}_title", Alias);
            }
        }

        public string GetTag(string text)
        {
            var code = text.ToLower().Trim().Replace(" ", "_");
            return string.Format("{0}_{1}", Alias, code);
        }

        public abstract AnalyzeResult Analyse(HtmlNode document);
    }
}
