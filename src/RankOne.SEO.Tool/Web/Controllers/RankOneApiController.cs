using System.Web.Http;
using RankOne.Business.Models;
using RankOne.Business.Services;
using Umbraco.Web.WebApi;

namespace RankOne.Web.Controllers
{
    public class RankOneApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public PageAnalysisVm AnalyzeUrl(int id, string focusKeyword = null)
        {
            var analyzeService = new AnalyzeService();
            var result =  analyzeService.AnalyzeWebPage(id, focusKeyword);

            var analysis = new PageAnalysisVm
            {
                AnalyzerResults = result.AnalyzerResults,
                HtmlResult = new HtmlResultVm
                {
                    Url = result.HtmlResult.Url,
                    Size = result.HtmlResult.Size
                },
                Status = result.Status
            };

            return analysis;
        }

        [HttpGet]
        public PageInformation GetPageInformation(int id)
        {
            var pageInformationService = new PageInformationService();
            var result = pageInformationService.GetpageInformation(id);
            return result;
        } 
    }
}
