using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Dictionary;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Profiling;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models.Fakes;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class ContentHelperTests
    {
        private readonly UmbracoHelper _umbracoHelper;

        public ContentHelperTests()
        {
            var databaseContext = new DatabaseContext(Mock.Of<IDatabaseFactory>(), Mock.Of<ILogger>(),
                new SqlSyntaxProviders(new[] {Mock.Of<ISqlSyntaxProvider>()}));

            var applicationContext = ApplicationContext.EnsureContext(
                databaseContext,
                new ServiceContext(),
                CacheHelper.CreateDisabledCacheHelper(),
                new ProfilingLogger(
                    Mock.Of<ILogger>(),
                    Mock.Of<IProfiler>()), true);

            var umbracoContext = UmbracoContext.EnsureContext(
                Mock.Of<HttpContextBase>(),
                applicationContext,
                new Mock<WebSecurity>(null, null).Object,
                Mock.Of<IUmbracoSettingsSection>(),
                new List<IUrlProvider>(), true);

            var mockedComponentRenderer = new Mock<IUmbracoComponentRenderer>();
            mockedComponentRenderer.Setup(x => x.RenderTemplate(1, null)).Returns(new HtmlString("<html>test</html>"));

            var urlProvider = new UrlProvider(umbracoContext,
                Mock.Of<IWebRoutingSection>(section => section.UrlProviderMode == UrlProviderMode.Auto.ToString()),
                new[] {Mock.Of<IUrlProvider>()});

            var membershipHelper = new MembershipHelper(umbracoContext, Mock.Of<MembershipProvider>(), Mock.Of<RoleProvider>());

            _umbracoHelper = new UmbracoHelper(umbracoContext, 
                Mock.Of<IPublishedContent>(),
                Mock.Of<ITypedPublishedContentQuery>(),
                Mock.Of<IDynamicPublishedContentQuery>(),
                Mock.Of<ITagQuery>(),
                Mock.Of<IDataTypeService>(),
                urlProvider,
                Mock.Of<ICultureDictionary>(),
                mockedComponentRenderer.Object,
                membershipHelper);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetNodeHtmlFromNullReturnsNull()
        {
            var contentHelper = new ContentHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(null);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void GetNodeHtmlFromPublishedContentWithNoTemplateIdReturnsNull()
        {
            IPublishedContent publishedContent = new StubPublishedContentBase
            {
                TemplateIdGet = () => 0,
                IdGet = () => 1
            };

            var contentHelper = new ContentHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(publishedContent);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void GetNodeHtmlFromPublishedContentWithIdReturnsNull()
        {
            IPublishedContent publishedContent = new StubPublishedContentBase
            {
                TemplateIdGet = () => 1,
                IdGet = () => 0
            };

            var contentHelper = new ContentHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(publishedContent);
        }

        [TestMethod]
        public void GetNodeHtmlFromPublishedContentWithIdReturnsObject()
        {
            IPublishedContent publishedContent = new StubPublishedContentBase
            {
                TemplateIdGet = () => 1,
                IdGet = () => 1
            };

            var contentHelper = new ContentHelper(_umbracoHelper);

            var result = contentHelper.GetNodeHtml(publishedContent);

            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("<html>test</html>", result);
        }
    }
}
