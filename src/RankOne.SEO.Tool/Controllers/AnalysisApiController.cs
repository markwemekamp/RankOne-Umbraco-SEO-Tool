using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Web.Http;
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
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;

        public AnalysisApiController() : this(RankOneContext.Instance)
        { }

        public AnalysisApiController(IRankOneContext rankOneContext) : this(rankOneContext.AnalyzeService.Value, rankOneContext.TypedPublishedContentQuery.Value)
        { }

        public AnalysisApiController(IAnalyzeService analyzeService, ITypedPublishedContentQuery typedPublishedContentQuery)
        {
            if (analyzeService == null) throw new ArgumentNullException(nameof(analyzeService));
            if (typedPublishedContentQuery == null) throw new ArgumentNullException(nameof(typedPublishedContentQuery));

            _analyzeService = analyzeService;
            _typedPublishedContentQuery = typedPublishedContentQuery;
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
        public IHttpActionResult AnalyzeNode(int id, string focusKeyword = null)
        {
            if (id < 1) return BadRequest();

            try
            {
                var node = _typedPublishedContentQuery.TypedContent(id);
                return Ok(_analyzeService.CreateAnalysis(node, focusKeyword));
            }
            catch (MissingFieldException ex)
            {
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(AnalysisApiController), "RankOne AnalyzeNode Exception", ex);
                return InternalServerError(ex);
            }
        }
    }
}