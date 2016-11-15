using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Services;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class AnalysisApiController : UmbracoAuthorizedApiController
    {
        private readonly IAnalyzeService _analyzeService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly AnalysisCacheService _analysisCacheService;

        public AnalysisApiController() : this(new AnalyzeService())
        { }

        public AnalysisApiController(IAnalyzeService analyzeService)
        {
            _analyzeService = analyzeService;
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _analysisCacheService = new AnalysisCacheService();
        }

        /// <summary>
        /// Analyzes the node.
        /// The focus keyword can be given as a parameter here for performance reasons and the check the node
        /// against specified keywords.
        /// If the keyword option is set to null, the AnalyzeService tries to determine if the keyword is set
        /// by analyzing the node's properties.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="focusKeyword">The focus keyword.</param>
        /// <returns></returns>
        [HttpGet]
        public PageAnalysis AnalyzeNode(int id, string focusKeyword = null)
        {
            try
            {
                var node = _umbracoHelper.TypedContent(id);
                var analysis = _analyzeService.CreateAnalysis(node, focusKeyword);
                _analysisCacheService.SaveCachedAnalysis(node.Id, focusKeyword, analysis);
                return analysis;
            }
            catch (MissingFieldException ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(message);
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(AnalysisApiController), "RankOne AnalyzeNode Exception", ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
