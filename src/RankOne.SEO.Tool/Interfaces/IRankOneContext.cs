using System;
using System.Collections.Generic;
using Umbraco.Web;

namespace RankOne.Interfaces
{
    public interface IRankOneContext
    {
        Lazy<IEnumerable<ISummary>> Summaries { get; }

        Lazy<UmbracoHelper> UmbracoHelper { get; }

        Lazy<UmbracoContext> UmbracoContext { get; }

        Lazy<ITypedPublishedContentQuery> TypedPublishedContentQuery { get; }

        Lazy<IUmbracoComponentRenderer> UmbracoComponentRenderery { get; }

        Lazy<ITemplateHelper> TemplateHelper { get; }

        Lazy<IDashboardSettingsSerializer> DashboardSettingsSerializer { get; }

        Lazy<INodeReportRepository> NodeReportRepository { get; }

        Lazy<IFocusKeywordHelper> FocusKeywordHelper { get; }

        Lazy<IByteSizeHelper> ByteSizeHelper { get; }

        Lazy<IConfigurationHelper> ConfigurationHelper { get; }

        Lazy<IPageScoreSerializer> PageScoreSerializer { get; }

        Lazy<IPageScoreNodeHelper> PageScoreNodeHelper { get; }

        Lazy<INodeReportTableHelper> NodeReportTableHelper { get; }

        Lazy<IHtmlTagHelper> HtmlTagHelper { get; }

        Lazy<IMinificationHelper> MinificationHelper { get; }

        Lazy<IEncodingHelper> EncodingHelper { get; }

        Lazy<IWordCounter> WordCounter { get; }

        Lazy<IOptionHelper> OptionHelper { get; }

        Lazy<IWebRequestHelper> WebRequestHelper { get; }

        Lazy<ICacheHelper> CacheHelper { get; }

        Lazy<IUrlHelper> UrlHelper { get; }

        Lazy<IScoreService> ScoreService { get; }

        Lazy<IPageAnalysisService> PageAnalysisService { get; }

        Lazy<IDashboardDataService> DashboardDataService { get; }

        Lazy<IPageInformationService> PageInformationService { get; }

        Lazy<IAnalyzeService> AnalyzeService { get; }

        Lazy<IUrlStatusService> UrlStatusService { get; }

        Lazy<IHttpWebRequestFactory> WebRequestFactory { get; }

        Lazy<IAnalysisCacheService> AnalysisCacheService { get; }
    }
}