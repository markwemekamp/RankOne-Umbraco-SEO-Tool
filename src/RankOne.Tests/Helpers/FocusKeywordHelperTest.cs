using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using Umbraco.Core.Models;
using Umbraco.Web.Models.Fakes;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class FocusKeywordHelperTest
    {
        [TestMethod]
        public void GetFocusKeywordReturnsFocusKeywordProperty()
        {
            using (ShimsContext.Create())
            {
                IPublishedContent publishedContent = new StubPublishedContentBase();

                // Fake HasProperty extension method
                Umbraco.Web.Fakes.ShimPublishedContentExtensions.HasPropertyIPublishedContentString = (node, alias) =>
                {
                    switch (alias)
                    {
                        case "focusKeyword":
                            return true;
                        default:
                            return false;
                    }
                };

                // Fake GetPropertyValue<string> extension method
                Umbraco.Web.Fakes.ShimPublishedContentExtensions.GetPropertyValueOf1IPublishedContentString(
                    (node, alias) =>
                    {
                        switch (alias)
                        {
                            case "focusKeyword":
                                return "test keyword";
                            default:
                                return null;
                        }
                    });

                var focusKeywordHelper = new FocusKeywordHelper();
                var result = focusKeywordHelper.GetFocusKeyword(publishedContent);

                Assert.AreEqual("test keyword", result);
            }
        }

        [TestMethod]
        public void GetFocusKeywordReturnsNullWithNoProperties()
        {
            using (ShimsContext.Create())
            {
                IPublishedContent publishedContent = new StubPublishedContentBase
                {
                    PropertiesGet = () => new List<IPublishedProperty>()
                };

                Umbraco.Web.Fakes.ShimPublishedContentExtensions.HasPropertyIPublishedContentString = (node, alias) =>
                {
                    return false;
                };

                var focusKeywordHelper = new FocusKeywordHelper();
                var result = focusKeywordHelper.GetFocusKeyword(publishedContent);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetFocusKeywordReturnsFocusKeywordFromDashboardProperty()
        {
            using (ShimsContext.Create())
            {
                var propertyMock = new Mock<IPublishedProperty>();
                propertyMock.SetupGet(x => x.HasValue).Returns(true);
                propertyMock.SetupGet(x => x.Value).Returns("{\"focusKeyword\": \"umbraco\"}");

                IPublishedContent publishedContent = new StubPublishedContentBase
                {
                    PropertiesGet = () => new List<IPublishedProperty> { propertyMock.Object }
                };

                Umbraco.Web.Fakes.ShimPublishedContentExtensions.HasPropertyIPublishedContentString = (node, alias) =>
                {
                    return false;
                };

                var focusKeywordHelper = new FocusKeywordHelper();
                var result = focusKeywordHelper.GetFocusKeyword(publishedContent);

                Assert.AreEqual("umbraco", result);
            }
        }

        [TestMethod]
        public void GetFocusKeywordReturnsNullFromEmptyDashboardProperty()
        {
            using (ShimsContext.Create())
            {
                var propertyMock = new Mock<IPublishedProperty>();
                propertyMock.SetupGet(x => x.HasValue).Returns(true);
                propertyMock.SetupGet(x => x.Value).Returns("{\"focusKeyword\": \"\"}");

                IPublishedContent publishedContent = new StubPublishedContentBase
                {
                    PropertiesGet = () => new List<IPublishedProperty> { propertyMock.Object }
                };

                Umbraco.Web.Fakes.ShimPublishedContentExtensions.HasPropertyIPublishedContentString = (node, alias) =>
                {
                    return false;
                };

                var focusKeywordHelper = new FocusKeywordHelper();
                var result = focusKeywordHelper.GetFocusKeyword(publishedContent);

                Assert.IsNull(result);
            }
        }
    }
}
