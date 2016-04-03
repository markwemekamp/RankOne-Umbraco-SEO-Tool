using System;
using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        public AnalyzeResult Analyse(XDocument document, string keyword)
        {
            var result = new AnalyzeResult();
            result.Title = "keywordtitleanalyzer_title";

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
                if (titleTags.First().Value.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = "keywordtitleanalyzer_title_contains_keyword",
                        Type = ResultType.Succes
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

        public override AnalyzeResult Analyse(XDocument document)
        {
            throw new System.NotImplementedException();
        }
    }
}
