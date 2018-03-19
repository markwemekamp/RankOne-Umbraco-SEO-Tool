using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class TitleAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new TitleAnalyzer(null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new TitleAnalyzer(new HtmlTagHelper(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new TitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }
    }
}
