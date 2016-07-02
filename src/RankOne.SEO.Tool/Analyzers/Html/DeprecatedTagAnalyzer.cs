using System.Linq;
using HtmlAgilityPack;
using RankOne.Models;

namespace RankOne.Analyzers.Html
{
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
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
                result.AddResultRule("deprecatedtaganalyzer_no_deprecated_tags_found", ResultType.Success);
            }

            return result;
        }

        private void CheckTag(HtmlNode document, string tagname, AnalyzeResult result)
        {
            var acronymTags = HtmlHelper.GetElements(document, tagname);

            if (acronymTags.Any())
            {
                result.AddResultRule(string.Format("deprecatedtaganalyzer_{0}_tag_found", tagname), ResultType.Warning);
            }
        }
    }
}
