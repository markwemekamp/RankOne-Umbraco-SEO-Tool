using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Interfaces;
using RankOne.Services;
using System;
using System.Collections.Generic;

namespace RankOne.Tests.Services
{
    [TestClass]
    public class PageAnalysisServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForScoreService_ThrowsException()
        {
            new PageAnalysisService(null, Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(), Moq.Mock.Of<ITemplateHelper>(), Moq.Mock.Of<INodeReportRepository>(), 
                Moq.Mock.Of<IPageScoreSerializer>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForByteSizeHelper_ThrowsException()
        {
            new PageAnalysisService(Moq.Mock.Of<IScoreService>(), null, new List<ISummary>(), Moq.Mock.Of<ITemplateHelper>(), Moq.Mock.Of<INodeReportRepository>(),
                Moq.Mock.Of<IPageScoreSerializer>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForSummaries_ThrowsException()
        {
            new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), null, Moq.Mock.Of<ITemplateHelper>(), 
                Moq.Mock.Of<INodeReportRepository>(), Moq.Mock.Of<IPageScoreSerializer>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForTemplateHelper_ThrowsException()
        {
            new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(), null, Moq.Mock.Of<INodeReportRepository>(),
                Moq.Mock.Of<IPageScoreSerializer>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForNodeReportRepository_ThrowsException()
        {
            new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(), Moq.Mock.Of<ITemplateHelper>(), null,
                Moq.Mock.Of<IPageScoreSerializer>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForPageScoreSerializer_ThrowsException()
        {
            new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(), Moq.Mock.Of<ITemplateHelper>(), 
                Moq.Mock.Of<INodeReportRepository>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Save_OnExectureWithMinus1ForId_ThrowsException()
        {
            var analysisCacheService = new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(), 
                Moq.Mock.Of<ITemplateHelper>(), Moq.Mock.Of<INodeReportRepository>(), Moq.Mock.Of<IPageScoreSerializer>());
            analysisCacheService.Save(-1, new RankOne.Models.PageAnalysis());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_OnExectureWithNullForPageAnalysis_ThrowsException()
        {
            var analysisCacheService = new PageAnalysisService(Moq.Mock.Of<IScoreService>(), Moq.Mock.Of<IByteSizeHelper>(), new List<ISummary>(),
                Moq.Mock.Of<ITemplateHelper>(), Moq.Mock.Of<INodeReportRepository>(), Moq.Mock.Of<IPageScoreSerializer>());
            analysisCacheService.Save(1, null);
        }
    }
}