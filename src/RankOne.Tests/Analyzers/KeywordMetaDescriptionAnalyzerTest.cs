using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Keywords;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class KeywordMetaDescriptionAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new KeywordMetaDescriptionAnalyzer((IHtmlTagHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new KeywordMetaDescriptionAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(null);
        }
    }
}
