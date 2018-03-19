using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class KeywordTitleAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new KeywordTitleAnalyzer((IHtmlTagHelper)null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordTitleAnalyzer(new HtmlTagHelper(), (IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordTitleAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null);
        }
    }
}
