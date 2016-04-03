using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class CssMinificationAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = "cssminificationanalyzer_title";

            var domain = "http://www.novaware.nl";

            var localCssFiles = HtmlHelper.GetElementsWithAttribute(document, "link", "href").
                Where(x => 
                        x.Attributes().Any(y => y.Name == "rel" && y.Value == "stylesheet") &&
                        x.Attributes().Any(y => y.Name == "href" && (y.Value.StartsWith("/")
                            || y.Value.StartsWith(domain)
                        ))
                );

            foreach (var localCssFile in localCssFiles)
            {
                var address = HtmlHelper.GetAttribute(localCssFile, "href");

                // TODO load file
                // check for /n
            }
            

            return result;
        }
    }
}
