using RankOne.Helpers;
using RankOne.Repositories;
using RankOne.Serializers;
using RankOne.Services;
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

        Lazy<TemplateHelper> TemplateHelper { get; }

        Lazy<DashboardSettingsSerializer> DashboardSettingsSerializer { get; }

        Lazy<NodeReportService> NodeReportService { get; }

        Lazy<FocusKeywordHelper> FocusKeywordHelper { get; }

        Lazy<HtmlHelper> HtmlHelper { get; }

        Lazy<ByteSizeHelper> ByteSizeHelper { get; }

        Lazy<ConfigurationHelper> ConfigurationHelper { get; }

        Lazy<PageScoreSerializer> PageScoreSerializer { get; }

        Lazy<PageScoreNodeHelper> PageScoreNodeHelper { get; }

        Lazy<NodeReportTableHelper> NodeReportTableHelper { get; }

        Lazy<HtmlTagHelper> HtmlTagHelper { get; }

        Lazy<MinificationHelper> MinificationHelper { get; }

        Lazy<EncodingHelper> EncodingHelper { get; }

        Lazy<WordCounter> WordCounter { get; }

        Lazy<OptionHelper> OptionHelper { get; }

        Lazy<WebRequestHelper> WebRequestHelper { get; }

        Lazy<CacheHelper> CacheHelper { get; }

        Lazy<UrlHelper> UrlHelper { get; }

        Lazy<ScoreService> ScoreService { get; }

        Lazy<PageAnalysisService> PageAnalysisService { get; }

        Lazy<DashboardDataService> DashboardDataService { get; }

        Lazy<PageInformationService> PageInformationService { get; }

        Lazy<AnalyzeService> AnalyzeService { get; }

        Lazy<UrlStatusService> UrlStatusService { get; }

        Lazy<AnalysisCacheRepository> AnalysisCacheRepository { get; }
    }
}
