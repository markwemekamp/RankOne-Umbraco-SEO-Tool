using System.Xml.Linq;
using RankOne.Business;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Models;

namespace SEO.Umbraco.Extensions.Analyzers
{
    public abstract class BaseAnalyzer
    {
        protected HtmlHelper HtmlHelper;

        protected BaseAnalyzer()
        {
            HtmlHelper = new HtmlHelper();
        }

        public abstract AnalyzeResult Analyse(XDocument document);
    }
}
