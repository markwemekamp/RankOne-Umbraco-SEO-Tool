using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Repositories;
using RankOne.Serializers;
using RankOne.Services;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Web;

namespace RankOne.Models
{
    public sealed class RankOneContext
    {
        private static volatile RankOneContext instance;
        private static object syncRoot = new object();

        public Lazy<IEnumerable<ISummary>> Summaries
        {
            get
            {
                return new Lazy<IEnumerable<ISummary>>(() => ConfigurationHelper.Value.GetSummaries());
            }

        }

        public Lazy<UmbracoHelper> UmbracoHelper
        {
            get
            {
                return new Lazy<UmbracoHelper>(() => new UmbracoHelper(UmbracoContext.Current));
            }
        }

        public Lazy<ITypedPublishedContentQuery> TypedPublishedContentQuery
        {
            get
            {
                return new Lazy<ITypedPublishedContentQuery>(() => UmbracoHelper.Value.ContentQuery);
            }
        }

        public Lazy<IUmbracoComponentRenderer> UmbracoComponentRenderery
        {
            get
            {
                return new Lazy<IUmbracoComponentRenderer>(() => UmbracoHelper.Value.UmbracoComponentRenderer);
            }
        }

        public Lazy<TemplateHelper> TemplateHelper
        {
            get
            {
                return new Lazy<TemplateHelper>(() => new TemplateHelper(UmbracoComponentRenderery.Value));
            }
        }

        public Lazy<DatabaseContext> DatabaseContext
        {
            get
            {
                return new Lazy<DatabaseContext>(() => UmbracoContext.Current.Application.DatabaseContext);
            }
        }

        public Lazy<DashboardSettingsSerializer> DashboardSettingsSerializer
        {
            get
            {
                return new Lazy<DashboardSettingsSerializer>(() => new DashboardSettingsSerializer());
            }
        }

        public Lazy<FocusKeywordHelper> FocusKeywordHelper
        {
            get
            {
                return new Lazy<FocusKeywordHelper>(() => new FocusKeywordHelper());
            }
        }

        public Lazy<HtmlHelper> HtmlHelper
        {
            get
            {
                return new Lazy<HtmlHelper>(() => new HtmlHelper());
            }
        }

        public Lazy<ByteSizeHelper> ByteSizeHelper
        {
            get
            {
                return new Lazy<ByteSizeHelper>(() => new ByteSizeHelper());
            }
        }

        public Lazy<ConfigurationHelper> ConfigurationHelper
        {
            get
            {
                return new Lazy<ConfigurationHelper>(() => new ConfigurationHelper());
            }
        }

        public Lazy<PageScoreSerializer> PageScoreSerializer
        {
            get
            {
                return new Lazy<PageScoreSerializer>(() => new PageScoreSerializer());
            }
        }

        public Lazy<PageScoreNodeHelper> PageScoreNodeHelper
        {
            get
            {
                return new Lazy<PageScoreNodeHelper>(() => new PageScoreNodeHelper());
            }
        }

        public Lazy<NodeReportTableHelper> NodeReportTableHelper
        {
            get
            {
                return new Lazy<NodeReportTableHelper>(() => new NodeReportTableHelper());
            }
        }

        public Lazy<HtmlTagHelper> HtmlTagHelper
        {
            get
            {
                return new Lazy<HtmlTagHelper>(() => new HtmlTagHelper());
            }
        }

        public Lazy<MinificationHelper> MinificationHelper
        {
            get
            {
                return new Lazy<MinificationHelper>(() => new MinificationHelper());
            }
        }

        public Lazy<EncodingHelper> EncodingHelper
        {
            get
            {
                return new Lazy<EncodingHelper>(() => new EncodingHelper());
            }
        }

        public Lazy<WordCounter> WordCounter
        {
            get
            {
                return new Lazy<WordCounter>(() => new WordCounter());
            }
        }

        public Lazy<OptionHelper> OptionHelper
        {
            get
            {
                return new Lazy<OptionHelper>(() => new OptionHelper());
            }
        }

        public Lazy<WebRequestHelper> WebRequestHelper
        {
            get
            {
                return new Lazy<WebRequestHelper>(() => new WebRequestHelper());
            }
        }

        public Lazy<Helpers.CacheHelper> CacheHelper
        {
            get
            {
                return new Lazy<Helpers.CacheHelper>(() => new Helpers.CacheHelper());
            }
        }

        public Lazy<UrlHelper> UrlHelper
        {
            get
            {
                return new Lazy<UrlHelper>(() => new UrlHelper());
            }
        }

        public Lazy<ScoreService> ScoreService
        {
            get
            {
                return new Lazy<ScoreService>(() => new ScoreService());
            }
        }

        public Lazy<PageAnalysisService> PageAnalysisService
        {
            get
            {
                return new Lazy<PageAnalysisService>(() => new PageAnalysisService());
            }
        }

        public Lazy<DashboardDataService> DashboardDataService
        {
            get
            {
                return new Lazy<DashboardDataService>(() => new DashboardDataService());
            }
        }

        public Lazy<PageInformationService> PageInformationService
        {
            get
            {
                return new Lazy<PageInformationService>(() => new PageInformationService());
            }
        }

        public Lazy<AnalyzeService> AnalyzeService
        {
            get
            {
                return new Lazy<AnalyzeService>(() => new AnalyzeService());
            }
        }

        public Lazy<UrlStatusService> UrlStatusService
        {
            get
            {
                return new Lazy<UrlStatusService>(() => new UrlStatusService());
            }
        }

        public Lazy<NodeReportRepository> NodeReportRepository
        {
            get
            {
                return new Lazy<NodeReportRepository>(() => new NodeReportRepository());
            }
        }

        public Lazy<AnalysisCacheRepository> AnalysisCacheRepository
        {
            get
            {
                return new Lazy<AnalysisCacheRepository>(() => new AnalysisCacheRepository());
            }
        }

        private RankOneContext()
        {
        }

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