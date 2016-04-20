using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class HeadingAnalyzer : BaseAnalyzer
    {
        public HeadingAnalyzer()
        {
            Alias = "headinganalyzer";
        }

        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult();
            result.Alias = Alias;
            result.Title = TitleTag;

            var h1Tags = HtmlHelper.GetElements(document, "h1");

            if (!h1Tags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("no h1 tag"), Type = ResultType.Warning });
            }
            else if (h1Tags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("multiple h1 tags"), Type = ResultType.Warning });
            }
            else
            {
                var resultRule = new ResultRule {Code = GetTag("one h1 tag"), Type = ResultType.Success};
                // ReSharper disable once PossibleNullReferenceException
                resultRule.Tokens.Add(h1Tags.FirstOrDefault().InnerText);
                result.ResultRules.Add(resultRule);
            }
            return result;
        }
    }
}
