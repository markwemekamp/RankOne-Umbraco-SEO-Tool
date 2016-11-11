using System;
using RankOne.Analyzers;
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

        private readonly DefinitionHelper _reflectionService;

        public BaseSummary()
        {
            _reflectionService = new DefinitionHelper();
        }

        public virtual Analysis GetAnalysis()
        {
            var analysis = new Analysis();
            var types = _reflectionService.GetAllAnalyzerTypesForSummary(Name);

            foreach (var type in types)
            {
                var instance = GetInstance(type);

                var result = GetResultFromAnalyzer(instance);
                analysis.Results.Add(result);
            }

            return analysis;
        }

        private BaseAnalyzer GetInstance(Type type)
        {
            var instance = Activator.CreateInstance(type);
            return (BaseAnalyzer)instance;
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
