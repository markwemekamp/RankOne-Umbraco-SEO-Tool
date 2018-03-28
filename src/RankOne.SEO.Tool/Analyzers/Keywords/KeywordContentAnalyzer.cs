using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace RankOne.Analyzers.Keywords
{
    public class KeywordContentAnalyzer : BaseAnalyzer
    {
        private readonly IHtmlTagHelper _htmlTagHelper;

        public KeywordContentAnalyzer() : this(RankOneContext.Instance)
        { }

        public KeywordContentAnalyzer(IRankOneContext rankOneContext) : this(rankOneContext.HtmlTagHelper.Value)
        { }

        public KeywordContentAnalyzer(IHtmlTagHelper htmlTagHelper) : base()
        {
            if (htmlTagHelper == null) throw new ArgumentNullException(nameof(htmlTagHelper));

            _htmlTagHelper = htmlTagHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            try
            {
                var bodyTag = _htmlTagHelper.GetBodyTag(pageData.Document);

                if (bodyTag != null)
                {
                    var bodyText = bodyTag.InnerText.Trim();
                    // replace multiple spaces with 1
                    var text = Regex.Replace(bodyText.ToLower(), @"\s+", " ");
                    var matches = Regex.Matches(text, pageData.Focuskeyword);

                    if (matches.Count == 0)
                    {
                        AddResultRule("content_doesnt_contain_keyword", ResultType.Warning);
                    }
                    else
                    {
                        var resultRule = new ResultRule
                        {
                            Alias = "content_contains_keyword",
                            Type = ResultType.Success
                        };
                        resultRule.Tokens.Add(matches.Count.ToString());
                        AddResultRule(resultRule);
                    }
                }
            }
            catch (NoElementFoundException e)
            {
                AddResultRule("no_" + e.ElementName + "_tag", ResultType.Error);
            }
            catch (MultipleElementsFoundException e)
            {
                AddResultRule("multiple_" + e.ElementName + "_tags", ResultType.Error);
            }
        }
    }
}