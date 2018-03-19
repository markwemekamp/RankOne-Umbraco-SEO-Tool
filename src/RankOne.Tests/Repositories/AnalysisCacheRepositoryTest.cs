using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Interfaces;
using RankOne.Repositories;
using RankOne.Serializers;
using System;

namespace RankOne.Tests.Repositories
{
    [TestClass]
    public class AnalysisCacheRepositoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForNodeReportRepository_ThrowsException()
        {
            new AnalysisCacheRepository(null, new PageScoreSerializer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExectureWithNullForPageScoreSerializer_ThrowsException()
        {
            new AnalysisCacheRepository(Mock.Of<INodeReportService>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Save_OnExectureWithMinus1ForId_ThrowsException()
        {
            var analysisCacheRepository = new AnalysisCacheRepository(Mock.Of<INodeReportService>(), new PageScoreSerializer());
            analysisCacheRepository.Save(-1, new RankOne.Models.PageAnalysis());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_OnExectureWithNullForPageAnalysis_ThrowsException()
        {
            var analysisCacheRepository = new AnalysisCacheRepository(Mock.Of<INodeReportService>(), new PageScoreSerializer());
            analysisCacheRepository.Save(1, null);
        }
    }
}
