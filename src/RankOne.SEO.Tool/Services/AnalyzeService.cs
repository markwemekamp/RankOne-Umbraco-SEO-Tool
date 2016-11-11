using System;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using RankOne.Helpers;
using RankOne.Models;
using RankOne.Summaries;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class AnalyzeService
    {
        private readonly HtmlDocument _htmlParser;
        private readonly ScoreService _scoreService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ContentHelper _contentHelper;
        private readonly AnalysisCacheService _analysisCacheService;
        private readonly FocusKeywordHelper _focusKeywordHelper;
        private readonly DefinitionHelper _reflectionService;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
            _scoreService = new ScoreService();
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _contentHelper = new ContentHelper(_umbracoHelper);
            _analysisCacheService = new AnalysisCacheService();
            _focusKeywordHelper = new FocusKeywordHelper();
            _reflectionService = new DefinitionHelper();
        }

        public PageAnalysis CreateAnalysis(int id, string focusKeyword = null)
        {
            var node = _umbracoHelper.TypedContent(id);
            return CreateAnalysis(node, focusKeyword);
        }

        public PageAnalysis CreateAnalysis(IPublishedContent node, string focusKeyword = null)
        {
            if (node.TemplateId == 0)
            {
                throw new MissingFieldException("TemplateId is not set");
            }

            if (!string.IsNullOrEmpty(focusKeyword))
            {
                focusKeyword = GetFocusKeywordFromProperties(node);
            }

            var pageAnalysis = GetPageAnalysis(node, focusKeyword);
            _analysisCacheService.SaveCachedAnalysis(node.Id, focusKeyword, pageAnalysis);
            return pageAnalysis;
        }

        private PageAnalysis GetPageAnalysis(IPublishedContent node, string focusKeyword)
        {
            var pageAnalysis = new PageAnalysis();

            try
            {
                var htmlString = GetTemplateHtml(node);
                var htmlResult = GetHtmlResult(htmlString);

                pageAnalysis.Url = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = GetHtmlSize(htmlString);

                SetAnalyzerResults(pageAnalysis, htmlResult);
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            return pageAnalysis;
        }

        private int GetHtmlSize(string htmlString)
        {
            if (htmlString == null)
            {
                return 0;
            }
            return Encoding.ASCII.GetByteCount(htmlString);
        }

        private HtmlResult GetHtmlResult(string htmlString)
        {
            var htmlNode = GetHtmlNode(htmlString);
            var htmlResult = new HtmlResult
            {
                Html = htmlString,
                Document = htmlNode
            };
            return htmlResult;
        }

        private HtmlNode GetHtmlNode(string htmlString)
        {
            if (htmlString != null)
            {
                _htmlParser.LoadHtml(htmlString);
                return _htmlParser.DocumentNode;
            }
            return null;
        }

        private string GetTemplateHtml(IPublishedContent node)
        {
            return _contentHelper.GetNodeHtml(node);
        }

        private string GetFocusKeywordFromProperties(IPublishedContent node)
        {
            return _focusKeywordHelper.GetFocusKeyword(node);
        }

        private void SetAnalyzerResults(PageAnalysis pageAnalysis, HtmlResult html)
        {
            // Get all types marked with the Summary attribute
            var summaryDefinitions = _reflectionService.GetSummaryDefinitions();

            // Instantiate the types and retrieve te results
            foreach (var summaryDefinition in summaryDefinitions)
            {
                var summary = CreateSummaryFromType(summaryDefinition.Type, pageAnalysis, html);

                var analyzerResult = new AnalyzerResult
                {
                    Alias = summaryDefinition.Summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.AnalyzerResults.Add(analyzerResult);
            }
        }

        private BaseSummary CreateSummaryFromType(Type type, PageAnalysis pageAnalysis, HtmlResult html)
        {
            var instance = Activator.CreateInstance(type);
            var summary = (BaseSummary)instance;
            summary.FocusKeyword = pageAnalysis.FocusKeyword;
            summary.HtmlResult = html;
            summary.Url = pageAnalysis.Url;
            return summary;
        }
    }
}
