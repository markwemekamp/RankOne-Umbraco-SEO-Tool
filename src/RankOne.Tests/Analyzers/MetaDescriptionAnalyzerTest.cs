using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class MetaDescriptionAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForHtmlTagHelper_ThrowArgumentNullException()
        {
            new MetaDescriptionAnalyzer((IHtmlTagHelper)null, new OptionHelper());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameterForOptionHelper_ThrowArgumentNullException()
        {
            new MetaDescriptionAnalyzer(new HtmlTagHelper(), (IOptionHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new MetaDescriptionAnalyzer(new HtmlTagHelper(), new OptionHelper());
            analyzer.Analyse(null); ;
        }
    }
}
