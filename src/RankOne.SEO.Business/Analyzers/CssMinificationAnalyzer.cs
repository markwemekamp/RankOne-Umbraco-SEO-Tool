using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class CssMinificationAnalyzer : BaseAnalyzer
    {
        public CssMinificationAnalyzer()
        {
            Alias = "cssminificationanalyzer";
        }

        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult();
            result.Title = TitleTag;

            var domain = "http://www.novaware.nl";

            var localCssFiles = HtmlHelper.GetElementsWithAttribute(document, "link", "href").
                Where(x => 
                        x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet") &&
                        x.Attributes.Any(y => y.Name == "href" && (y.Value.StartsWith("/")
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
