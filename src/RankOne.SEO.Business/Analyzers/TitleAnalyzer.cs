using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class TitleAnalyzer : BaseAnalyzer
    {
        public TitleAnalyzer()
        {
            Alias = "titleanalyzer";
        }

        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult();
            result.Alias = Alias;

            var headTag = HtmlHelper.GetElements(document, "head");
            var titleTags = HtmlHelper.GetElements(headTag.First(), "title");

            if (!titleTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("no title tag"), Type = ResultType.Error });
            }
            else if (titleTags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag("multiple title tags"), Type = ResultType.Error });
            }
            else
            {
                var firstTitleTag = titleTags.FirstOrDefault();
                if (firstTitleTag != null)
                {
                    var titleValue = firstTitleTag.InnerText;

                    if (string.IsNullOrWhiteSpace(titleValue))
                    {
                        result.ResultRules.Add(new ResultRule { Code = GetTag("no title value"), Type = ResultType.Error });
                    }
                    else
                    {
                        titleValue = titleValue.Trim();

                        if (titleValue.Length > 60)
                        {
                            result.ResultRules.Add(new ResultRule { Code = GetTag("title too long"), Type = ResultType.Warning});
                        }

                        if (titleValue.Length < 10)
                        {
                            result.ResultRules.Add(new ResultRule { Code = GetTag("title too short"), Type = ResultType.Warning });
                        }
                        else if (titleValue.Length < 40)
                        {
                            result.ResultRules.Add(new ResultRule { Code = GetTag("title less than 40 characters"), Type = ResultType.Warning });
                        }

                        if (titleValue.Length <= 60 && titleValue.Length >= 40)
                        {
                            result.ResultRules.Add(new ResultRule { Code = GetTag("title more than 40 less than 60 characters"), Type = ResultType.Success });
                        }
                    }
                }
            }
            return result;
        }
    }
}
