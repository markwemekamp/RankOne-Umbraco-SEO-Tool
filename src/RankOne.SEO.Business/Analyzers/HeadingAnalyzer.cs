using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class HeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult
            {
                Alias = "headinganalyzer"
            };

            var h1Tags = HtmlHelper.GetElements(document, "h1");

            if (!h1Tags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "headinganalyzer_no_h1_tag", Type = ResultType.Warning });
            }
            else if (h1Tags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = "headinganalyzer_multiple_h1_tags", Type = ResultType.Warning });
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
