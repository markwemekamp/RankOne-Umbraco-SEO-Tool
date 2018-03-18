using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class KeywordContentAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordContentAnalyzer((IHtmlTagHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordContentAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(null); ;
        }
    }
}
