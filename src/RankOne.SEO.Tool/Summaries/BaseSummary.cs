using RankOne.Analyzers;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RankOne.Summaries
{
    public class BaseSummary
    {
        public HtmlResult HtmlResult { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }

        private readonly IDefintionHelper _definitionHelper;

        public BaseSummary() : this(new DefinitionHelper())
        { }

        public BaseSummary(IDefintionHelper defintionHelper)
        {
            _definitionHelper = defintionHelper;
        }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();
            var types = _definitionHelper.GetAllAnalyzerTypesForSummary(Name);

            foreach (var type in types)
            {
                var instance = type.GetInstance<BaseAnalyzer>();

                var analyzerCategory = type.FirstAttribute<AnalyzerCategory>();

                var result = GetResultFromAnalyzer(instance);

                result.Alias = analyzerCategory.Alias;
                analysis.Results.Add(result);
            }

            return analysis;
        }

        private AnalyzeResult GetResultFromAnalyzer(BaseAnalyzer baseAnalyzerInstance)
        {
            var pageData = new PageData
            {
                Document = HtmlResult.Document,
                Focuskeyword = FocusKeyword,
                Url = Url
            };
            return baseAnalyzerInstance.Analyse(pageData);
        }
    }
}
