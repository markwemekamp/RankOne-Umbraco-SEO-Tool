using RankOne.Interfaces;
using RankOne.Models;
using System.Text.RegularExpressions;

namespace RankOne.Analyzers.Keywords
{
    public class KeywordContentAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;

        public KeywordContentAnalyzer() : this(RankOneContext.Instance)
        { }

        public KeywordContentAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public KeywordContentAnalyzer(IHtmlTagHelper htmlTagHelper)
        {
            _htmlTagHelper = htmlTagHelper;
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var bodyTag = _htmlTagHelper.GetBodyTag(pageData.Document, result);

            if (bodyTag != null)
            {
                var bodyText = bodyTag.InnerText.Trim();
                var text = Regex.Replace(bodyText.ToLower(), @"\s+", " ");
                var matches = Regex.Matches(text, pageData.Focuskeyword);

                if (matches.Count == 0)
                {
                    result.AddResultRule("content_doesnt_contain_keyword", ResultType.Warning);
                }
                else
                {
                    var resultRule = new ResultRule
                    {
                        Alias = "content_contains_keyword",
                        Type = ResultType.Success
                    };
                    resultRule.Tokens.Add(matches.Count.ToString());
                    result.ResultRules.Add(resultRule);
                }
            }

            return result;
        }
    }
}