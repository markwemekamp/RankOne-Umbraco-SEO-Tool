using System.Linq;
using System.Xml.Linq;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers
{
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        public DeprecatedTagAnalyzer()
        {
            Alias = "deprecatedtaganalyzer";
        }

        public override AnalyzeResult Analyse(XDocument document)
        {
            var result = new AnalyzeResult();
            result.Title = TitleTag;

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
                result.ResultRules.Add(new ResultRule { Code = GetTag("no deprecated tags found"), Type = ResultType.Succes});
            }

            return result;
        }

        private void CheckTag(XDocument document, string tagname, AnalyzeResult result)
        {
            var acronymTags = HtmlHelper.GetElements(document, tagname);

            if (acronymTags.Any())
            {
                result.ResultRules.Add(new ResultRule { Code = GetTag(string.Format("{0}_tag_found", tagname)), Type = ResultType.Warning});
            }
        }
    }
}
