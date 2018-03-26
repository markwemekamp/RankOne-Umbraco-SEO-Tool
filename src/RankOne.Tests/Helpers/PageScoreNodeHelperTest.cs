using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Serializers;
using RankOne.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class PageScoreNodeHelperTest
    {
        private Mock<ITypedPublishedContentQuery> _typedPublishedContentQueryMock;
        private Mock<INodeReportService> _nodeReportServiceMock;
        private Mock<IPageScoreSerializer>  _pageScoreSerializerMock;
        private Mock<IAnalyzeService> _analyzeServiceMock;
        private PageScoreNodeHelper _mockedPageScoreNodeHelper;

        [TestInitialize]
        public void Initialize()
        {
            _typedPublishedContentQueryMock = new Mock<ITypedPublishedContentQuery>();
            _nodeReportServiceMock = new Mock<INodeReportService>();
            _nodeReportServiceMock.Setup(x => x.GetById(1)).Returns(new NodeReport() { Id = 1, FocusKeyword = "focus", Report = "" });
            _nodeReportServiceMock.Setup(x => x.GetById(11)).Returns((NodeReport)null);
            _nodeReportServiceMock.Setup(x => x.GetById(12)).Returns(new NodeReport() { Id = 12, FocusKeyword = "focus", Report = "" });
            _pageScoreSerializerMock = new Mock<IPageScoreSerializer>();
            _analyzeServiceMock = new Mock<IAnalyzeService>();

            _mockedPageScoreNodeHelper = new PageScoreNodeHelper(_typedPublishedContentQueryMock.Object, _nodeReportServiceMock.Object, _pageScoreSerializerMock.Object,
                _analyzeServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForTypedPublishedContentQuery_ThrowsException()
        {
            new PageScoreNodeHelper(null, new Mock<INodeReportService>().Object, new PageScoreSerializer(), new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForNodeReportService_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, null, new PageScoreSerializer(), new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForPageScoreSerializer_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, new Mock<INodeReportService>().Object, null, new Mock<IAnalyzeService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForAnalyzeService_ThrowsException()
        {
            new PageScoreNodeHelper(new Mock<ITypedPublishedContentQuery>().Object, new Mock<INodeReportService>().Object, new PageScoreSerializer(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPageScoresFromCache_OnExecuteWithNull_ThrowsException()
        {
            _mockedPageScoreNodeHelper.GetPageScoresFromCache(null);
        }

        [TestMethod]
        public void GetPageScoresFromCache_OnExecute_ReturnsPageScoreNode()
        {
            var nodes = new List<IPublishedContent>()
            {
                new PublishedContentMock(){
                    Id = 1,
                    Name = "node 1",
                    TemplateId = 99,
                    Children = new List<IPublishedContent> ()
                    {
                        new PublishedContentMock(){
                            Id = 11,
                            Name = "node 11",
                            TemplateId = 0,
                        },
                        new PublishedContentMock(){
                            Id = 12,
                            Name = "node 12",
                            TemplateId = 99,
                        }
                    }
                }
            };

            var result = _mockedPageScoreNodeHelper.GetPageScoresFromCache(nodes);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().Children.Count());
            _nodeReportServiceMock.Verify(x => x.GetById(1), Times.Once);
            _nodeReportServiceMock.Verify(x => x.GetById(11), Times.Once);
            _nodeReportServiceMock.Verify(x => x.GetById(12), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdatePageScores_OnExecuteWithNull_ThrowsException()
        {
            _mockedPageScoreNodeHelper.UpdatePageScores(null);
        }

        [TestMethod]
        public void UpdatePageScores_OnExecute_ReturnsPageScoreNode()
        {
            var nodes = new List<IPublishedContent>()
            {
                new PublishedContentMock(){
                    Id = 1,
                    Name = "node 1",
                    TemplateId = 99,
                    Children = new List<IPublishedContent> ()
                    {
                        new PublishedContentMock(){
                            Id = 11,
                            Name = "node 11",
                            TemplateId = 0,
                        },
                        new PublishedContentMock(){
                            Id = 12,
                            Name = "node 12",
                            TemplateId = 99,
                        }
                    }
                }
            };

            var result = _mockedPageScoreNodeHelper.UpdatePageScores(nodes);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().Children.Count());
            _analyzeServiceMock.Verify(x => x.CreateAnalysis(It.Is<IPublishedContent>(y => y.Id == 1), null), Times.Once);
            _analyzeServiceMock.Verify(x => x.CreateAnalysis(It.Is<IPublishedContent>(y => y.Id == 11), null), Times.Never);
            _analyzeServiceMock.Verify(x => x.CreateAnalysis(It.Is<IPublishedContent>(y => y.Id == 12), null), Times.Once);
        }
    }
}
