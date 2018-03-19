using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class AdditionalCallAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new AdditionalCallAnalyzer((IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new AdditionalCallAnalyzer(new OptionHelper());
            analyzer.Analyse(null);
        }
    }
}
