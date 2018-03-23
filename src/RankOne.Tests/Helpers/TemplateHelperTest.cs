using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
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
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class TemplateHelperTest
    {
        private UmbracoHelper _umbracoHelper;

        [TestInitialize]
        public void Initialize()
        {
            var databaseContext = new DatabaseContext(Moq.Mock.Of<IDatabaseFactory>(), Moq.Mock.Of<ILogger>(),
                new SqlSyntaxProviders(new[] { Moq.Mock.Of<ISqlSyntaxProvider>() }));

            var applicationContext = ApplicationContext.EnsureContext(
                databaseContext,
                new ServiceContext(),
                Umbraco.Core.CacheHelper.CreateDisabledCacheHelper(),
                new ProfilingLogger(
                    Moq.Mock.Of<ILogger>(),
                    Moq.Mock.Of<IProfiler>()), true);

            var umbracoContext = UmbracoContext.EnsureContext(
                Moq.Mock.Of<HttpContextBase>(),
                applicationContext,
                new Mock<WebSecurity>(null, null).Object,
                Moq.Mock.Of<IUmbracoSettingsSection>(),
                new List<IUrlProvider>(), true);

            var mockedComponentRenderer = new Mock<IUmbracoComponentRenderer>();
            mockedComponentRenderer.Setup(x => x.RenderTemplate(1, null)).Returns(new HtmlString("<html>test</html>"));

            var urlProvider = new UrlProvider(umbracoContext,
                Moq.Mock.Of<IWebRoutingSection>(section => section.UrlProviderMode == UrlProviderMode.Auto.ToString()),
                new[] { Moq.Mock.Of<IUrlProvider>() });

            var membershipHelper = new MembershipHelper(umbracoContext, Moq.Mock.Of<MembershipProvider>(), Moq.Mock.Of<RoleProvider>());

            _umbracoHelper = new UmbracoHelper(umbracoContext,
                Moq.Mock.Of<IPublishedContent>(),
                Moq.Mock.Of<ITypedPublishedContentQuery>(),
                Moq.Mock.Of<IDynamicPublishedContentQuery>(),
                Moq.Mock.Of<ITagQuery>(),
                Moq.Mock.Of<IDataTypeService>(),
                urlProvider,
                Moq.Mock.Of<ICultureDictionary>(),
                mockedComponentRenderer.Object,
                membershipHelper);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNodeHtml_OnExecuteWithNullParameter_ThrowsException()
        {
            var contentHelper = new TemplateHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(null);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void GetNodeHtml_OnExecuteWithPublishedContentWithNoTemplateId_ReturnsNull()
        {
            var publishedContent = new PublishedContentMock()
            {
                TemplateId = 0,
                Id = 1
            };

            var contentHelper = new TemplateHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(publishedContent);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void GetNodeHtml_OnExecuteWithPublishedContentWithId_ReturnsNull()
        {
            var publishedContent = new PublishedContentMock()
            {
                TemplateId = 1,
                Id = 0
            };

            var contentHelper = new TemplateHelper(_umbracoHelper);

            contentHelper.GetNodeHtml(publishedContent);
        }

        [TestMethod]
        public void GetNodeHtml_WithPublishedContentWithId_ReturnsObject()
        {
            var publishedContent = new PublishedContentMock()
            {
                TemplateId = 1,
                Id = 1
            };

            var contentHelper = new TemplateHelper(_umbracoHelper);

            var result = contentHelper.GetNodeHtml(publishedContent);

            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("<html>test</html>", result);
        }
    }
}