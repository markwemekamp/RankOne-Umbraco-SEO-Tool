using System.Linq;
using System.Text.RegularExpressions;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordcontentanalyzer")]
    public class KeywordContentAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult
            {
                Alias = "keywordcontentanalyzer"
            };

            var bodyTags = pageData.Document.GetDescendingElements("body");

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

                if (bodyTag != null)
                {
                    var bodyText = bodyTag.InnerText.Trim();
                    var text = Regex.Replace(bodyText.ToLower(), @"\s+", " ");
                    var matches = Regex.Matches(text, pageData.Focuskeyword);

                    if (matches.Count == 0)
                    {
                        result.AddResultRule("keywordcontentanalyzer_content_doesnt_contain_keyword", ResultType.Warning);
                    }
                    else
                    {
                        var resultRule = new ResultRule
                        {
                            Alias = "keywordcontentanalyzer_content_contains_keyword",
                            Type = ResultType.Success
                        };
                        resultRule.Tokens.Add(matches.Count.ToString());
                        result.ResultRules.Add(resultRule);
                    }
                }
            }

            return result;
        }
    }
}
