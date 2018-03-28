using Moq;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Profiling;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace RankOne.Tests.Controllers
{
    public class BaseUmbracoTestController
    {
        public BaseUmbracoTestController()
        {
            EnsureUmbracoContext();
        }

        public void EnsureUmbracoContext()
        {
            var appContext = ApplicationContext.EnsureContext(new DatabaseContext(Moq.Mock.Of<IDatabaseFactory2>(), Moq.Mock.Of<ILogger>(), 
                new SqlSyntaxProviders(new[] { Moq.Mock.Of<ISqlSyntaxProvider>() })), new ServiceContext(), CacheHelper.CreateDisabledCacheHelper(), new ProfilingLogger(
                    Moq.Mock.Of<ILogger>(), Moq.Mock.Of<IProfiler>()), true);

            var context = UmbracoContext.EnsureContext(Moq.Mock.Of<HttpContextBase>(), appContext, new Mock<WebSecurity>(null, null).Object, Moq.Mock.Of<IUmbracoSettingsSection>(), 
                Enumerable.Empty<IUrlProvider>(), true);
        }
    }
}
