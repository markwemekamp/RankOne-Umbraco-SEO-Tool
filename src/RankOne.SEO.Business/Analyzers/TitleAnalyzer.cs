using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;
using SEO.Umbraco.Extensions.Analyzers;

namespace RankOne.Business.Analyzers
{
    public class TitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = "titleanalyzer_title";

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
                    var titleValue = firstTitleTag.Value;

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
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_less_than_40", Type = ResultType.Warning });
                        }

                        if (titleValue.Length <= 60 && titleValue.Length >= 40)
                        {
                            result.ResultRules.Add(new ResultRule { Code = "titleanalyzer_title_more_than_40_less_than_60", Type = ResultType.Succes });
                        }
                    }
                }
            }
            return result;
        }
    }
}
