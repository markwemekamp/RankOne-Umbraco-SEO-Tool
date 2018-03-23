using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [TestMethod]
        public void Options_OnGets_ReturnDefaultValues()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper());

            Assert.AreEqual(12, analyzer.DeprecatedTags.Count());
            Assert.IsTrue(analyzer.DeprecatedTags.Contains("center"));
        }

        [TestMethod]
        public void Options_OnGetWithOverridenValues_ReturnOverridenValues()
        {
            var analyzer = new DeprecatedTagAnalyzer(new OptionHelper())
            {
                Options = new List<IOption>()
                {
                    new Option(){ Key = "DeprecatedTags", Value = "div"}
                }
            };

            Assert.IsTrue(analyzer.DeprecatedTags.Contains("div"));
            Assert.AreEqual(1, analyzer.DeprecatedTags.Count());
        }
    }
}
