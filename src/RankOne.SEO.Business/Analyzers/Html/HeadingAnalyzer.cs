using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
{
    public class HeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "headinganalyzer"
            };

            var h1Tags = HtmlHelper.GetElements(document, "h1");

            if (!h1Tags.Any())
            {
                result.AddResultRule("headinganalyzer_no_h1_tag", ResultType.Warning);
            }
            else if (h1Tags.Count() > 1)
            {
                result.AddResultRule("headinganalyzer_multiple_h1_tags", ResultType.Warning);
            }
            else
            {
                var resultRule = new ResultRule {Code = "headinganalyzer_one_h1_tag", Type = ResultType.Success};
                // ReSharper disable once PossibleNullReferenceException
                resultRule.Tokens.Add(h1Tags.FirstOrDefault().InnerText);
                result.ResultRules.Add(resultRule);
            }
            return result;
        }
    }
}
