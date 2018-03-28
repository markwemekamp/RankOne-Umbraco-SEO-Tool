using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class PageApiController : UmbracoAuthorizedApiController
    {
        private readonly IPageInformationService _pageInformationService;

        public PageApiController() : this(RankOneContext.Instance)
        { }

        public PageApiController(IRankOneContext rankOneContext) : this(rankOneContext.PageInformationService.Value)
        { }

        public PageApiController(IPageInformationService pageInformationService)
        {
            if (pageInformationService == null) throw new ArgumentNullException(nameof(pageInformationService));

            _pageInformationService = pageInformationService;
        }

        [HttpGet]
        public IHttpActionResult GetPageInformation(int id)
        {
            if (id < 1) return BadRequest();

            try
            {
                return Ok(_pageInformationService.GetpageInformation(id));
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(AnalysisApiController), "RankOne GetPageInformation Exception", ex);
                return InternalServerError(ex);
            }
        }
    }
}