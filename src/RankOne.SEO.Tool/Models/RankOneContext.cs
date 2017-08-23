using RankOne.Helpers;
using RankOne.Repositories;
using RankOne.Serializers;
using RankOne.Services;
using System;
using Umbraco.Core;
using Umbraco.Web;

namespace RankOne.Models
{
    public sealed class RankOneContext
    {
        private static volatile RankOneContext instance;
        private static object syncRoot = new object();

        public Lazy<UmbracoHelper> UmbracoHelper => new Lazy<UmbracoHelper>(() => new UmbracoHelper(UmbracoContext.Current));
        public Lazy<ITypedPublishedContentQuery> TypedPublishedContentQuery => new Lazy<ITypedPublishedContentQuery>(() => UmbracoHelper.Value.ContentQuery);
        public Lazy<IUmbracoComponentRenderer> UmbracoComponentRenderery => new Lazy<IUmbracoComponentRenderer>(() => UmbracoHelper.Value.UmbracoComponentRenderer);
        public Lazy<TemplateHelper> TemplateHelper => new Lazy<TemplateHelper>(() => new TemplateHelper(UmbracoComponentRenderery.Value));
        public Lazy<DatabaseContext> DatabaseContext => new Lazy<DatabaseContext>(() => UmbracoContext.Current.Application.DatabaseContext);

        public Lazy<DashboardSettingsSerializer> DashboardSettingsSerializer => new Lazy<DashboardSettingsSerializer>(() => new DashboardSettingsSerializer());
        public Lazy<FocusKeywordHelper> FocusKeywordHelper => new Lazy<FocusKeywordHelper>(() => new FocusKeywordHelper());
        public Lazy<HtmlHelper> HtmlHelper => new Lazy<HtmlHelper>(() => new HtmlHelper());
        public Lazy<ByteSizeHelper> ByteSizeHelper => new Lazy<ByteSizeHelper>(() => new ByteSizeHelper());
        public Lazy<ConfigurationHelper> ConfigurationHelper => new Lazy<ConfigurationHelper>(() => new ConfigurationHelper());
        public Lazy<PageScoreSerializer> PageScoreSerializer => new Lazy<PageScoreSerializer>(() => new PageScoreSerializer());
        public Lazy<PageScoreNodeHelper> PageScoreNodeHelper => new Lazy<PageScoreNodeHelper>(() => new PageScoreNodeHelper());
        public Lazy<NodeReportTableHelper> NodeReportTableHelper => new Lazy<NodeReportTableHelper>(() => new NodeReportTableHelper());
        public Lazy<HtmlTagHelper> HtmlTagHelper => new Lazy<HtmlTagHelper>(() => new HtmlTagHelper());
        public Lazy<MinificationHelper> MinificationHelper => new Lazy<MinificationHelper>(() => new MinificationHelper());
        public Lazy<EncodingHelper> EncodingHelper => new Lazy<EncodingHelper>(() => new EncodingHelper());
        public Lazy<WordCounter> WordCounter => new Lazy<WordCounter>(() => new WordCounter());
        public Lazy<OptionHelper> OptionHelper => new Lazy<OptionHelper>(() => new OptionHelper());
        public Lazy<WebRequestHelper> WebRequestHelper => new Lazy<WebRequestHelper>(() => new WebRequestHelper());
        

        public Lazy<ScoreService> ScoreService => new Lazy<ScoreService>(() => new ScoreService());
        public Lazy<PageAnalysisService> PageAnalysisService => new Lazy<PageAnalysisService>(() => new PageAnalysisService());
        public Lazy<DashboardDataService> DashboardDataService => new Lazy<DashboardDataService>(() => new DashboardDataService());
        public Lazy<PageInformationService> PageInformationService => new Lazy<PageInformationService>(() => new PageInformationService());
        public Lazy<AnalyzeService> AnalyzeService => new Lazy<AnalyzeService>(() => new AnalyzeService());
        public Lazy<UrlStatusService> UrlStatusService => new Lazy<UrlStatusService>(() => new UrlStatusService());

        public Lazy<NodeReportRepository> NodeReportRepository => new Lazy<NodeReportRepository>(() => new NodeReportRepository());
        public Lazy<AnalysisCacheRepository> AnalysisCacheRepository => new Lazy<AnalysisCacheRepository>(() => new AnalysisCacheRepository());

        private RankOneContext() { }

        public static RankOneContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new RankOneContext();
                    }
                }

                return instance;
            }
        } 
    }
}
