using System.Web.Http;
using RankOne.Business.Models;
using RankOne.Business.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace RankOne.Web.Controllers
{
	[PluginController("RankOne")]
    public class RankOneApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public PageAnalysis AnalyzeUrl(string url)
        {
            var analyzeService = new AnalyzeService();
            return analyzeService.AnalyzeWebPage(url);
        }
    }
}
