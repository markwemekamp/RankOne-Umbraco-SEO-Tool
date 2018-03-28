using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Services;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class BrokenLinkAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlStatusService_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(null, new UrlHelper(), new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlHelper_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(new UrlStatusService(RankOneContext.Instance), null, new CacheHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForCacheHelper_ThrowArgumentNullException()
        {
            new BrokenLinkAnalyzer(new UrlStatusService(RankOneContext.Instance), new UrlHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new BrokenLinkAnalyzer(new UrlStatusService(RankOneContext.Instance), new UrlHelper(), new CacheHelper());
            analyzer.Analyse(null);
        }
    }
}
