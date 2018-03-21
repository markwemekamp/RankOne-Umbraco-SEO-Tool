using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class CssMinificationAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForMinificationHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(null, new CacheHelper(), new UrlHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForCacheHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(new MinificationHelper(), null, new UrlHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForUrlHelper_ThrowArgumentNullException()
        {
            new CssMinificationAnalyzer(new MinificationHelper(), new CacheHelper(), null);
        }
    }
}
