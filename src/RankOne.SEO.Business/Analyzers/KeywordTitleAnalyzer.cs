using System;
using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class KeywordTitleAnalyzer : BaseAnalyzer
    {
        public KeywordTitleAnalyzer()
        {
            Alias = "keywordtitleanalyzer";
        }

        public AnalyzeResult Analyse(HtmlNode document, string keyword)
        {
            var result = new AnalyzeResult();
            result.Title = TitleTag;

            var titleTags = HtmlHelper.GetElements(document, "title");

            if (!titleTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("no title tag"), Type = ResultType.Warning });
            }
            else if (titleTags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("multiple title tags"), Type = ResultType.Warning });
            }
            else
            {
                if (titleTags.First().InnerText.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = GetTag("title contains keyword"),
                        Type = ResultType.Succes
                    });
                }
                else
                {
                    result.ResultRules.Add(new ResultRule
                    {
                        Code = GetTag("title doesnt contain keyword"),
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
