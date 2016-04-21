using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        public AnalyzeResult Analyse(HtmlNode document, string keyword)
        {
            var result = new AnalyzeResult();
            result.Alias = "keywordtitleanalyzer";

            var titleTags = HtmlHelper.GetElements(document, "title");

            if (!titleTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "keywordtitleanalyzer_no_title_tag", Type = ResultType.Warning });
            }
            else if (titleTags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = "keywordtitleanalyzer_multiple_title_tags", Type = ResultType.Warning });
            }
            else
            {
                if (titleTags.First().InnerText.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = "keywordtitleanalyzer_title_contains_keyword",
                        Type = ResultType.Success
                    });
                }
                else
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = "keywordtitleanalyzer_title_doesnt_contain_keyword",
                        Type = ResultType.Hint
                    });
                }
            }
          
            return result;
        }

        public override AnalyzeResult Analyse(HtmlNode document)
        {
            throw new System.NotImplementedException();
        }
    }
}
