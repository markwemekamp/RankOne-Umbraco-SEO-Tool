using RankOne.Factories;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Repositories;
using RankOne.Serializers;
using RankOne.Services;
using System;
using System.Collections.Generic;
using Umbraco.Web;

namespace RankOne.Models
{
    public sealed class RankOneContext : IRankOneContext
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
                return new Lazy<UmbracoHelper>(() => new UmbracoHelper(UmbracoContext.Value));
            }
        }

        public Lazy<UmbracoContext> UmbracoContext
        {
            get
            {
                return new Lazy<UmbracoContext>(() => Umbraco.Web.UmbracoContext.Current);
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

        public Lazy<ITemplateHelper> TemplateHelper
        {
            get
            {
                return new Lazy<ITemplateHelper>(() => new TemplateHelper(UmbracoComponentRenderery.Value));
            }
        }

        public Lazy<IDashboardSettingsSerializer> DashboardSettingsSerializer
        {
            get
            {
                return new Lazy<IDashboardSettingsSerializer>(() => new DashboardSettingsSerializer());
            }
        }

        public Lazy<INodeReportRepository> NodeReportRepository
        {
            get
            {
                return new Lazy<INodeReportRepository>(() => new NodeReportRepository());
            }
        }

        public Lazy<IFocusKeywordHelper> FocusKeywordHelper
        {
            get
            {
                return new Lazy<IFocusKeywordHelper>(() => new FocusKeywordHelper());
            }
        }

        public Lazy<IByteSizeHelper> ByteSizeHelper
        {
            get
            {
                return new Lazy<IByteSizeHelper>(() => new ByteSizeHelper());
            }
        }

        public Lazy<IConfigurationHelper> ConfigurationHelper
        {
            get
            {
                return new Lazy<IConfigurationHelper>(() => new ConfigurationHelper());
            }
        }

        public Lazy<IPageScoreSerializer> PageScoreSerializer
        {
            get
            {
                return new Lazy<IPageScoreSerializer>(() => new PageScoreSerializer());
            }
        }

        public Lazy<IPageScoreNodeHelper> PageScoreNodeHelper
        {
            get
            {
                return new Lazy<IPageScoreNodeHelper>(() => new PageScoreNodeHelper());
            }
        }

        public Lazy<INodeReportTableHelper> NodeReportTableHelper
        {
            get
            {
                return new Lazy<INodeReportTableHelper>(() => new NodeReportTableHelper());
            }
        }

        public Lazy<IHtmlTagHelper> HtmlTagHelper
        {
            get
            {
                return new Lazy<IHtmlTagHelper>(() => new HtmlTagHelper());
            }
        }

        public Lazy<IMinificationHelper> MinificationHelper
        {
            get
            {
                return new Lazy<IMinificationHelper>(() => new MinificationHelper());
            }
        }

        public Lazy<IEncodingHelper> EncodingHelper
        {
            get
            {
                return new Lazy<IEncodingHelper>(() => new EncodingHelper());
            }
        }

        public Lazy<IWordCounter> WordCounter
        {
            get
            {
                return new Lazy<IWordCounter>(() => new WordCounter());
            }
        }

        public Lazy<IOptionHelper> OptionHelper
        {
            get
            {
                return new Lazy<IOptionHelper>(() => new OptionHelper());
            }
        }

        public Lazy<IWebRequestHelper> WebRequestHelper
        {
            get
            {
                return new Lazy<IWebRequestHelper>(() => new WebRequestHelper());
            }
        }

        public Lazy<ICacheHelper> CacheHelper
        {
            get
            {
                return new Lazy<ICacheHelper>(() => new CacheHelper());
            }
        }

        public Lazy<IUrlHelper> UrlHelper
        {
            get
            {
                return new Lazy<IUrlHelper>(() => new UrlHelper());
            }
        }

        public Lazy<IScoreService> ScoreService
        {
            get
            {
                return new Lazy<IScoreService>(() => new ScoreService());
            }
        }

        public Lazy<IPageAnalysisService> PageAnalysisService
        {
            get
            {
                return new Lazy<IPageAnalysisService>(() => new PageAnalysisService());
            }
        }

        public Lazy<IDashboardDataService> DashboardDataService
        {
            get
            {
                return new Lazy<IDashboardDataService>(() => new DashboardDataService());
            }
        }

        public Lazy<IPageInformationService> PageInformationService
        {
            get
            {
                return new Lazy<IPageInformationService>(() => new PageInformationService());
            }
        }

        public Lazy<IAnalyzeService> AnalyzeService
        {
            get
            {
                return new Lazy<IAnalyzeService>(() => new AnalyzeService());
            }
        }

        public Lazy<IUrlStatusService> UrlStatusService
        {
            get
            {
                return new Lazy<IUrlStatusService>(() => new UrlStatusService());
            }
        }

        public Lazy<IAnalysisCacheService> AnalysisCacheService
        {
            get
            {
                return new Lazy<IAnalysisCacheService>(() => new AnalysisCacheService());
            }
        }

        public Lazy<IHttpWebRequestFactory> WebRequestFactory
        {
            get
            {
                return new Lazy<IHttpWebRequestFactory>(() => new WebRequestFactory());
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