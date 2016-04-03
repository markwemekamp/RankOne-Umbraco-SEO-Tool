using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class HeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = "headinganalyzer_title";

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
                var resultRule = new ResultRule {Code = "headinganalyzer_one_h1_tag", Type = ResultType.Succes};
                resultRule.Tokens.Add(h1Tags.FirstOrDefault().Value);
                result.ResultRules.Add(resultRule);
            }
            return result;
        }
    }
}
