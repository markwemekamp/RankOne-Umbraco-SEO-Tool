using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document)
        {
            var result = new AnalyzeResult
            {
                Alias = "deprecatedtaganalyzer"
            };

            CheckTag(document, "acronym", result);
            CheckTag(document, "applet", result);
            CheckTag(document, "basefont", result);
            CheckTag(document, "big", result);
            CheckTag(document, "center", result);
            CheckTag(document, "dir", result);
            CheckTag(document, "font", result);
            CheckTag(document, "frame", result);
            CheckTag(document, "frameset", result);
            CheckTag(document, "noframes", result);
            CheckTag(document, "strike", result);
            CheckTag(document, "tt", result);

            if (!result.ResultRules.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = "deprecatedtaganalyzer_no_deprecated_tags_found", Type = ResultType.Success});
            }

            return result;
        }

        private void CheckTag(HtmlNode document, string tagname, AnalyzeResult result)
        {
            var acronymTags = HtmlHelper.GetElements(document, tagname);

            if (acronymTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = string.Format("deprecatedtaganalyzer_{0}_tag_found", tagname), Type = ResultType.Warning});
            }
        }
    }
}
