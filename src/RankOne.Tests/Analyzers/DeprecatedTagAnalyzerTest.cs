using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class DeprecatedTagAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new DeprecatedTagAnalyzer((IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());
            analyzer.Analyse(null);
        }
    }
}
