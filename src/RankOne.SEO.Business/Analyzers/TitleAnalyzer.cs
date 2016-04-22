using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class TitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "titleanalyzer"
            };

            var headTag = HtmlHelper.GetElements(document, "head");
            var titleTags = HtmlHelper.GetElements(headTag.First(), "title");

            if (!titleTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_no_title_tag", Type = ResultType.Error });
            }
            else if (titleTags.Count() > 1)
            {
                result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_multiple_title_tags", Type = ResultType.Error });
            }
            else
            {
                var firstTitleTag = titleTags.FirstOrDefault();
                if (firstTitleTag != null)
                {
                    var titleValue = firstTitleTag.InnerText;

                    if (string.IsNullOrWhiteSpace(titleValue))
                    {
                        result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_no_title_value", Type = ResultType.Error });
                    }
                    else
                    {
                        titleValue = titleValue.Trim();

                        if (titleValue.Length > 60)
                        {
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_too_long", Type = ResultType.Warning});
                        }

                        if (titleValue.Length < 10)
                        {
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_too_short", Type = ResultType.Warning });
                        }
                        else if (titleValue.Length < 40)
                        {
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_less_than_40_characters", Type = ResultType.Warning });
                        }

                        if (titleValue.Length <= 60 && titleValue.Length >= 40)
                        {
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_more_than_40_less_than_60_characters", Type = ResultType.Success });
                        }
                    }
                }
            }
            return result;
        }
    }
}
