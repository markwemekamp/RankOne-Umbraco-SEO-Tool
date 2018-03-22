using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Tests.Mock;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class FocusKeywordHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowsException()
        {
            new FocusKeywordHelper((IDashboardSettingsSerializer)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFocusKeyword_OnExectureWithNullParameter_ThrowsException()
        {
            var dashboardSettingsSerializerMock = new Mock<IDashboardSettingsSerializer>();
            var focusKeywordHelper = new FocusKeywordHelper(dashboardSettingsSerializerMock.Object);
            focusKeywordHelper.GetFocusKeyword(null);
        }

        [TestMethod]
        public void GetFocusKeywordReturnsFocusKeywordProperty()
        {
            var mockedIPublishedContent = new PublishedContentMock();
            mockedIPublishedContent.Properties = new List<IPublishedProperty>() { new PublishedPropertyMock() { PropertyTypeAlias = "focusKeyword", Value = "test keyword" } };
            var dashboardSettingsSerializerMock = new Mock<IDashboardSettingsSerializer>();
            var focusKeywordHelper = new FocusKeywordHelper(dashboardSettingsSerializerMock.Object);
            var result = focusKeywordHelper.GetFocusKeyword(mockedIPublishedContent);

            Assert.AreEqual("test keyword", result);
        }

        [TestMethod]
        public void GetFocusKeywordReturnsNullWithNoProperties()
        {
            var mockedIPublishedContent = new PublishedContentMock();
            mockedIPublishedContent.Properties = new List<IPublishedProperty>() { };

            var focusKeywordHelper = new FocusKeywordHelper();
            var result = focusKeywordHelper.GetFocusKeyword(mockedIPublishedContent);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFocusKeywordReturnsFocusKeywordFromDashboardProperty()
        {
            var mockedIPublishedContent = new PublishedContentMock();
            mockedIPublishedContent.Properties = new List<IPublishedProperty>() { new PublishedPropertyMock() { PropertyTypeAlias = "otherProperty", Value = "{\"focusKeyword\": \"umbraco\"}" } };
            var dashboardSettingsSerializerMock = new Mock<IDashboardSettingsSerializer>();

            var focusKeywordHelper = new FocusKeywordHelper();
            var result = focusKeywordHelper.GetFocusKeyword(mockedIPublishedContent);

            Assert.AreEqual("umbraco", result);
        }

        [TestMethod]
        public void GetFocusKeywordReturnsNullFromEmptyDashboardProperty()
        {
            var mockedIPublishedContent = new PublishedContentMock();
            mockedIPublishedContent.Properties = new List<IPublishedProperty>() { new PublishedPropertyMock() { PropertyTypeAlias = "otherProperty", Value = "{\"focusKeyword\": \"\"}" } };
            var dashboardSettingsSerializerMock = new Mock<IDashboardSettingsSerializer>();

            var focusKeywordHelper = new FocusKeywordHelper();
            var result = focusKeywordHelper.GetFocusKeyword(mockedIPublishedContent);

            Assert.IsNull(result);
        }
    }
}