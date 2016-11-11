using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RankOne.Models;
using RankOne.Services;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class AnalysisApiController : UmbracoAuthorizedApiController
    {
        private readonly AnalyzeService _analyzeService;

        public AnalysisApiController()
        {
            _analyzeService = new AnalyzeService();
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
                _analyzeService.FocusKeyword = focusKeyword;
                return _analyzeService.CreateAnalysis(id);
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
