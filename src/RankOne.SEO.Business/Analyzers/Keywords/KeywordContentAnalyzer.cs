using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Keywords
{
    public class KeywordContentAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordcontentanalyzer"
            };

            var keyword = additionalValues[0].ToString();

            var bodyTags = HtmlHelper.GetElements(document, "body");

            if (!bodyTags.Any())
            {
                result.AddResultRule("keywordcontentanalyzer_no_body_tag", ResultType.Warning);
            }
            else if (bodyTags.Count() > 1)
            {
                result.AddResultRule("keywordcontentanalyzer_multiple_body_tags", ResultType.Warning);
            }
            else
            {
                var bodyTag = bodyTags.FirstOrDefault();

                var text = Regex.Replace(bodyTag.InnerText.Trim().ToLower(), @"\s+", " + ");

                var matches = Regex.Matches(text, keyword);

                if (matches.Count == 0)
                {
                    result.AddResultRule("keywordcontentanalyzer_content_doesnt_contain_keyword", ResultType.Warning);
                }
                else
                {
                    var resultRule = new ResultRule();
                    resultRule.Code = "keywordcontentanalyzer_content_contains_keyword";
                    resultRule.Type = ResultType.Success;
                    resultRule.Tokens.Add(matches.Count.ToString());
                    result.ResultRules.Add(resultRule);
                }
            }

            return result;
        }
    }
}
