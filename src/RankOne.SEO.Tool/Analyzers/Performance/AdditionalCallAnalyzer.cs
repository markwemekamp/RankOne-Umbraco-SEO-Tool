using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class AdditionalCallAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var cssFiles = pageData.Document.GetElementsWithAttribute("link", "href").
                Where(x => x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet"));
            var scriptFiles = pageData.Document.GetElementsWithAttribute("script", "src");
            var images = pageData.Document.GetElementsWithAttribute("img", "src");
            var objects = pageData.Document.GetElementsWithAttribute("object ", "data");

            var total = cssFiles.Count() + scriptFiles.Count() + images.Count() + objects.Count();

            var resultRule = new ResultRule();

            if (total > 30)
            {
                resultRule.Alias = "more_than_30_calls";
                resultRule.Type = ResultType.Warning;
            }
            else if (total > 15)
            {
                resultRule.Alias = "more_than_15_calls";
                resultRule.Type = ResultType.Hint;
            }
            else
            {
                resultRule.Alias = "less_than_15_calls";
                resultRule.Type = ResultType.Success;
            }

            resultRule.Tokens.Add(total.ToString());
            resultRule.Tokens.Add(cssFiles.Count().ToString());
            resultRule.Tokens.Add(scriptFiles.Count().ToString());
            resultRule.Tokens.Add(images.Count().ToString());
            resultRule.Tokens.Add(objects.Count().ToString());

            result.ResultRules.Add(resultRule);

            return result;
        }
    }
}