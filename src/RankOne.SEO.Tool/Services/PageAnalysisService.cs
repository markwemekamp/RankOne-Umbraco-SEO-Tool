using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Helpers;
using RankOne.Models;
using RankOne.Summaries;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageAnalysisService
    {
        
        private readonly ScoreService _scoreService;

        private readonly DefinitionHelper _reflectionService;
        private readonly HtmlHelper _htmlHelper;
        private readonly ByteSizeHelper _byteSizeHelper;

        public PageAnalysisService()
        {
            _scoreService = new ScoreService();

            _reflectionService = new DefinitionHelper();
            _htmlHelper = new HtmlHelper();     
            _byteSizeHelper = new ByteSizeHelper();     
        }

        public PageAnalysis CreatePageAnalysis(IPublishedContent node, string focusKeyword)
        {
            var pageAnalysis = new PageAnalysis();

            try
            {
                var htmlString = _htmlHelper.GetTemplateHtml(node);
                var htmlResult = _htmlHelper.GetHtmlResult(htmlString);

                pageAnalysis.Url = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = _byteSizeHelper.GetByteSize(htmlString);

                SetAnalyzerResults(pageAnalysis, htmlResult);
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            return pageAnalysis;
        }

        private void SetAnalyzerResults(PageAnalysis pageAnalysis, HtmlResult html)
        {
            // Get all types marked with the Summary attribute
            var summaryDefinitions = _reflectionService.GetSummaryDefinitions();

            // Instantiate the types and retrieve te results
            foreach (var summaryDefinition in summaryDefinitions)
            {
                var summary = summaryDefinition.Type.GetInstance<BaseSummary>();
                summary.FocusKeyword = pageAnalysis.FocusKeyword;
                summary.HtmlResult = html;
                summary.Url = pageAnalysis.Url;

                var analyzerResult = new AnalyzerResult
                {
                    Alias = summaryDefinition.Summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.AnalyzerResults.Add(analyzerResult);
            }
        }
    }
}
