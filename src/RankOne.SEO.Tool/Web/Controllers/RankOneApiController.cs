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
        public PageAnalysisVm AnalyzeUrl(string url, string focusKeyword = null)
        {
            var analyzeService = new AnalyzeService();
            var result =  analyzeService.AnalyzeWebPage(url, focusKeyword);

            var analysis = new PageAnalysisVm
            {
                AnalyzerResults = result.AnalyzerResults,
                HtmlResult = new HtmlResultVm
                {
                    ServerResponseTime = result.HtmlResult.ServerResponseTime,
                    Url = result.HtmlResult.Url,
                    Size = result.HtmlResult.Size
                },
                Status = result.Status,
                Url = result.Url
            };

            return analysis;
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
