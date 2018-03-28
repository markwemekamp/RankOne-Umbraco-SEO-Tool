using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class DashboardApiController : UmbracoAuthorizedApiController
    {
        private readonly IDashboardDataService _dashboardDataService;

        public DashboardApiController() : this(RankOneContext.Instance)
        { }

        public DashboardApiController(IRankOneContext rankOneContext) : this(rankOneContext.DashboardDataService.Value)
        { }

        public DashboardApiController(IDashboardDataService dashboardDataService)
        {
            if (dashboardDataService == null) throw new ArgumentNullException(nameof(dashboardDataService));

            _dashboardDataService = dashboardDataService;
        }

        [HttpGet]
        public IHttpActionResult Initialize()
        {
            _dashboardDataService.Initialize();
            return UpdateAllPages();
        }

        [HttpGet]
        public IHttpActionResult GetPageHierarchy()
        {
            return Ok(_dashboardDataService.GetHierarchyFromCache());
        }

        [HttpGet]
        public IHttpActionResult UpdateAllPages()
        {
            return Ok(_dashboardDataService.GetUpdatedHierarchy());
        }
    }
}