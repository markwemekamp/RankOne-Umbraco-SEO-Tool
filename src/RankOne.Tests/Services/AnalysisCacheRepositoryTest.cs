using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Interfaces;
using RankOne.Serializers;
using RankOne.Services;
using System;

namespace RankOne.Tests.Services
{
    [TestClass]
    public class AnalysisCacheServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForNodeReportRepository_ThrowsException()
        {
            new AnalysisCacheService(null, new PageScoreSerializer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForPageScoreSerializer_ThrowsException()
        {
            new AnalysisCacheService(Moq.Mock.Of<INodeReportRepository>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Save_OnExectureWithMinus1ForId_ThrowsException()
        {
            var analysisCacheRepository = new AnalysisCacheService(Moq.Mock.Of<INodeReportRepository>(), new PageScoreSerializer());
            analysisCacheRepository.Save(-1, new RankOne.Models.PageAnalysis());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_OnExectureWithNullForPageAnalysis_ThrowsException()
        {
            var analysisCacheService = new AnalysisCacheService(Moq.Mock.Of<INodeReportRepository>(), new PageScoreSerializer());
            analysisCacheService.Save(1, null);
        }
    }
}