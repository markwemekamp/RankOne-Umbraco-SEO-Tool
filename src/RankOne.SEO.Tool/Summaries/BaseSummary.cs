using System;
using RankOne.Analyzers;
using RankOne.ExtensionMethods;
using RankOne.Helpers;
using RankOne.Models;

namespace RankOne.Summaries
{
    public class BaseSummary
    {
        public HtmlResult HtmlResult { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string FocusKeyword { get; set; }

        private readonly DefinitionHelper _definitionHelper;

        public BaseSummary()
        {
            _definitionHelper = new DefinitionHelper();
        }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();
            var types = _definitionHelper.GetAllAnalyzerTypesForSummary(Name);

            foreach (var type in types)
            {
                var instance = type.GetInstance<BaseAnalyzer>();

                var result = GetResultFromAnalyzer(instance);
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
