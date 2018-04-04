using RankOne.Controllers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.UI.JavaScript;

namespace RankOne.Events
{
    public class ParsingApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> serverVars)
        {
            if (HttpContext.Current == null) throw new InvalidOperationException("HttpContext is null");

            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            serverVars.Add("RankOne", new Dictionary<string, object>
            {
                {"DashboardApi", urlHelper.GetUmbracoApiServiceBaseUrl<DashboardApiController>("Index")},
                {"AnalysisApi", urlHelper.GetUmbracoApiServiceBaseUrl<AnalysisApiController>("Index")},
                {"AnalyzerStructureApi", urlHelper.GetUmbracoApiServiceBaseUrl<AnalyzerStructureApiController>("Index")},
                {"PageApi", urlHelper.GetUmbracoApiServiceBaseUrl<PageApiController>("Index")}
            });
        }
    }
}
