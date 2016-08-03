using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using RankOne.Helpers;
using RankOne.Models;
using RankOne.Repositories;
using RankOne.Summaries;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class AnalyzeService
    {
        private readonly HtmlDocument _htmlParser;
        private NodeReportRepository _nodeReportRepository;
        private ScoreService _scoreService;
        private readonly JavaScriptSerializer _javascriptSerializer;
        private UmbracoHelper _umbracoHelper;
        private ContentHelper _contentHelper;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
            _nodeReportRepository = new NodeReportRepository();
            _scoreService = new ScoreService();
            _javascriptSerializer = new JavaScriptSerializer();
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _contentHelper = new ContentHelper(_umbracoHelper);
        }

        public PageAnalysis AnalyzeWebPage(int id, string focusKeyword = null)
        {
            var pageAnalysis = new PageAnalysis();
            try
            {
                var node = _umbracoHelper.TypedContent(id);
                var htmlString = _contentHelper.GetNodeHtml(node);

                if (string.IsNullOrEmpty(focusKeyword))
                {
                    focusKeyword = GetFocusKeyword(node);
                }

                _htmlParser.LoadHtml(htmlString);

                pageAnalysis.Url = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = Encoding.ASCII.GetByteCount(htmlString);

                var html = new HtmlResult
                {
                    Html = htmlString,
                    Document = _htmlParser.DocumentNode
                };


                var keywordAnalyzer = new KeywordSummary(html);
                keywordAnalyzer.FocusKeyword = focusKeyword;
                keywordAnalyzer.Url = pageAnalysis.Url;
                pageAnalysis.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "keywordanalyzer",
                    Analysis = keywordAnalyzer.GetAnalysis()
                });

                var htmlAnalyzer = new HtmlSummary(html);
                pageAnalysis.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "htmlanalyzer",
                    Analysis = htmlAnalyzer.GetAnalysis()
                });

                var performanceAnalyzer = new PerformanceSummary(html);
                performanceAnalyzer.Url = pageAnalysis.Url;
                pageAnalysis.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "performanceanalyzer",
                    Analysis = performanceAnalyzer.GetAnalysis()
                });
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            if (_nodeReportRepository.DatabaseExists())
            {
                var serializer = new JavaScriptSerializer();

                var json = serializer.Serialize(pageAnalysis.Score);

                var nodeReport = _nodeReportRepository.GetById(id);
                if (nodeReport == null)
                {
                    nodeReport = new NodeReport
                    {
                        Id = id,
                        FocusKeyword = focusKeyword,
                        Report = json
                    };

                    _nodeReportRepository.Insert(nodeReport);
                }
                else
                {
                    nodeReport.FocusKeyword = focusKeyword;
                    nodeReport.Report = json;

                    _nodeReportRepository.Update(nodeReport);
                }
            }

            return pageAnalysis;
        }

        private string GetFocusKeyword(IPublishedContent node)
        {
            // Try property focusKeyword
            if (node.HasProperty("focusKeyword"))
            {
                return node.GetPropertyValue<string>("focusKeyword");
            }

            // Try to figure out if there's a property of type RankOneDashboard on the node
            foreach (var property in node.Properties)
            {
                if (property.HasValue && property.Value.ToString().Contains("focusKeyword"))
                {
                    var dashboardSettings = _javascriptSerializer.Deserialize<DashboardSettings>(property.Value.ToString());
                    if (dashboardSettings != null)
                    {
                        return dashboardSettings.FocusKeyword;
                    }
                }
            }
            return null;
        }
    }
}
