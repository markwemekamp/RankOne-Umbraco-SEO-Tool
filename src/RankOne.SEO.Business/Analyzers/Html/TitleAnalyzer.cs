using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
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
                result.AddResultRule("titleanalyzer_no_title_tag", ResultType.Error);
            }
            else if (titleTags.Count() > 1)
            {
                result.AddResultRule("titleanalyzer_multiple_title_tags", ResultType.Error);
            }
            else
            {
                var firstTitleTag = titleTags.FirstOrDefault();
                if (firstTitleTag != null)
                {
                    var titleValue = firstTitleTag.InnerText;

                    if (string.IsNullOrWhiteSpace(titleValue))
                    {
                        result.AddResultRule("titleanalyzer_no_title_value", ResultType.Error);
                    }
                    else
                    {
                        titleValue = titleValue.Trim();

                        if (titleValue.Length > 60)
                        {
                            result.AddResultRule("titleanalyzer_title_too_long", ResultType.Warning);
                        }

                        if (titleValue.Length < 10)
                        {
                            result.AddResultRule("titleanalyzer_title_too_short", ResultType.Warning);
                        }
                        else if (titleValue.Length < 40)
                        {
                            result.AddResultRule("titleanalyzer_title_less_than_40_characters", ResultType.Warning);
                        }

                        if (titleValue.Length <= 60 && titleValue.Length >= 40)
                        {
                            result.AddResultRule("titleanalyzer_title_more_than_40_less_than_60_characters", ResultType.Success);
                        }
                    }
                }
            }
            return result;
        }
    }
}
