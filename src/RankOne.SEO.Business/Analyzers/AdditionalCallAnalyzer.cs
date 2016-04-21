using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class AdditionalCallAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult();
            result.Alias = "additionalcallanalyzer";

            var cssFiles = HtmlHelper.GetElementsWithAttribute(document, "link", "href").
                Where(x => x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet"));
            var scriptFiles = HtmlHelper.GetElementsWithAttribute(document, "script", "src");
            var images = HtmlHelper.GetElementsWithAttribute(document, "img", "src");
            var objects = HtmlHelper.GetElementsWithAttribute(document, "object ", "data");

            var total = cssFiles.Count() + scriptFiles.Count() + images.Count() + objects.Count();

            var resultRule = new ResultRule();

            if (total > 30)
            {
                resultRule.Code = "additionalcallanalyzer_more_than_30_calls";
                resultRule.Type = ResultType.Warning;
            }
            else if(total > 15)
            {
                resultRule.Code = "additionalcallanalyzer_more_than_15_calls";
                resultRule.Type = ResultType.Hint;
            }
            else
            {
                resultRule.Code = "additionalcallanalyzer_less_than_15_calls";
                resultRule.Type = ResultType.Success;
            }

            resultRule.Tokens.Add(cssFiles.Count().ToString());
            resultRule.Tokens.Add(scriptFiles.Count().ToString());
            resultRule.Tokens.Add(images.Count().ToString());
            resultRule.Tokens.Add(objects.Count().ToString());

            result.ResultRules.Add(resultRule);

            return result;
        }
    }
}
