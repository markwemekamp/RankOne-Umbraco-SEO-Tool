using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Performance;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class HtmlSizeAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForByteSizeHelper_ThrowArgumentNullException()
        {
            new HtmlSizeAnalyzer(new ByteSizeHelper(), (IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new HtmlSizeAnalyzer((IByteSizeHelper)null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new HtmlSizeAnalyzer(new ByteSizeHelper(), new OptionHelper());
            analyzer.Analyse(null); ;
        }
    }
}
