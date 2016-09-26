using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using RankOne.Attributes;
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
        private readonly JavaScriptSerializer _javascriptSerializer;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ContentHelper _contentHelper;
        private readonly NodeReportService _nodeReportService;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
            _scoreService = new ScoreService();
            _javascriptSerializer = new JavaScriptSerializer();
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _contentHelper = new ContentHelper(_umbracoHelper);
            _nodeReportService = new NodeReportService();
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

                // Get all types marked with the Summary attribute
                var currentAssembly = Assembly.GetExecutingAssembly();
                var types = currentAssembly.GetTypes()
                    .Where(
                        x => Attribute.IsDefined(x, typeof(Summary))).Select(x => new
                        {
                            Type = x,
                            Summary = (Summary)Attribute.GetCustomAttributes(x).FirstOrDefault(y => y is Summary)
                        }).OrderBy(x => x.Summary.SortOrder);

                // Instantiate the types and retrieve te results
                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type.Type);
                    var summary = (BaseSummary) instance;
                    summary.FocusKeyword = focusKeyword;
                    summary.HtmlResult = html;
                    summary.Url = pageAnalysis.Url;

                    pageAnalysis.AnalyzerResults.Add(new AnalyzerResult
                    {
                        Alias = type.Summary.Alias,
                        Analysis = summary.GetAnalysis()
                    });
                }
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            _nodeReportService.Save(id, focusKeyword, pageAnalysis);

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
