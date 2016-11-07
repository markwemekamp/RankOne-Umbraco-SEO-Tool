using System.Linq;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Models;

namespace RankOne.Analyzers.Performance
{
    [AnalyzerCategory(SummaryName = "Performance", Alias = "additionalcallanalyzer")]
    public class AdditionalCallAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(PageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "additionalcallanalyzer"
            };

            var cssFiles = pageData.Document.GetDescendingElementsWithAttribute("link", "href").
                Where(x => x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet"));
            var scriptFiles = pageData.Document.GetDescendingElementsWithAttribute("script", "src");
            var images = pageData.Document.GetDescendingElementsWithAttribute("img", "src");
            var objects = pageData.Document.GetDescendingElementsWithAttribute("object ", "data");

            var total = cssFiles.Count() + scriptFiles.Count() + images.Count() + objects.Count();

            var resultRule = new ResultRule();

            if (total > 30)
            {
                resultRule.Alias = "additionalcallanalyzer_more_than_30_calls";
                resultRule.Type = ResultType.Warning;
            }
            else if(total > 15)
            {
                resultRule.Alias = "additionalcallanalyzer_more_than_15_calls";
                resultRule.Type = ResultType.Hint;
            }
            else
            {
                resultRule.Alias = "additionalcallanalyzer_less_than_15_calls";
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
