using System.Web.Http;
using RankOne.Business.Models;
using RankOne.Business.Services;
using Umbraco.Web.WebApi;

namespace RankOne.Web.Controllers
{
	//[PluginController("RankOne")]
    public class RankOneApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public PageAnalysis AnalyzeUrl(string url, string focusKeyword = null)
        {
            var analyzeService = new AnalyzeService();
            var result =  analyzeService.AnalyzeWebPage(url, focusKeyword);
            if (result.HtmlResult != null)
            {
                result.HtmlResult.Document = null;
            }
            return result;
        }

        [HttpGet]
        public PageInformation GetPageInformation(string url)
        {
            var pageInformationService = new PageInformationService();
            var result = pageInformationService.GetpageInformation(url);
            return result;
        } 
    }
}
