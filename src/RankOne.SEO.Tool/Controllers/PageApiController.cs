using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Services;
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

        public PageApiController() : this(new PageInformationService())
        { }

        public PageApiController(IPageInformationService pageInformationService)
        {
            _pageInformationService = pageInformationService;
        }

        [HttpGet]
        public PageInformation GetPageInformation(int id)
        {
            try
            {
                return _pageInformationService.GetpageInformation(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(AnalysisApiController), "RankOne GetPageInformation Exception", ex);
                throw;
            }
        }
    }
}