using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Analyzers.Template;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Tests.Analyzers
{
    [TestClass]
    public class MetaRobotsAnalyzerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            new MetaRobotsAnalyzer((IHtmlTagHelper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyse_OnExecuteWithNullParameter_ThrowArgumentNullException()
        {
            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(null);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div>focus</div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("no_meta_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithNoMetaRobotsTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"description\" content=\"test\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Success, result.ResultRules.First().Type);
            Assert.AreEqual("no_robots_tag", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMulitpleRobotsTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"robots\" content=\"\" /><meta name=\"robots\" content=\"\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_robots_tags", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMulitpleGooglebotsTags_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<div><meta name=\"googlebot\" content=\"\" /><meta name=\"googlebot\" content=\"\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("multiple_googlebot_tags", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNone_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"none\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("robots_none", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNoIndex_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"noindex\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_index", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNoFollow_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"nofollow\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_follow", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNoSnippet_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"nosnippet\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_snippet", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNoODP_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"noodp\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_odp", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetToNoArchive_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"noarchive\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_archive", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetAnavailableAfter_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"unavailable_after\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("robots_unavailable_after", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsValueSetNoImageIndex_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"noimageindex\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("robots_no_image_index", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaRobotsWithMultipleValuesSet_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"robots\" content=\"unavailable_after,noimageindex,none\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 3);
            Assert.IsTrue(result.ResultRules.Any(x => x.Type == ResultType.Error));
            Assert.IsTrue(result.ResultRules.Any(x => x.Alias == "robots_none"));
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNone_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"none\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_none", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNoIndex_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"noindex\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Error, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_index", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNoFollow_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"nofollow\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Warning, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_follow", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNoSnippet_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"nosnippet\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_snippet", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNoODP_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"noodp\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_odp", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetToNoArchive_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"noarchive\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_archive", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetAnavailableAfter_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"unavailable_after\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_unavailable_after", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotValueSetNoImageIndex_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"noimageindex\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 1);
            Assert.AreEqual(ResultType.Information, result.ResultRules.First().Type);
            Assert.AreEqual("googlebot_no_image_index", result.ResultRules.First().Alias);
        }

        [TestMethod]
        public void Analyse_OnExecuteWithMetaGooglebotWithMultipleValuesSet_SetsAnalyzeResult()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($"<div><meta name=\"googlebot\" content=\"unavailable_after,noimageindex,none\" /></div>");

            var pageData = new PageData()
            {
                Document = doc.DocumentNode,
                Focuskeyword = "focus",
                Url = "http://www.google.com"
            };

            var analyzer = new MetaRobotsAnalyzer(new HtmlTagHelper());
            analyzer.Analyse(pageData);
            var result = analyzer.AnalyzeResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResultRules.Count == 3);
            Assert.IsTrue(result.ResultRules.Any(x => x.Type == ResultType.Error));
            Assert.IsTrue(result.ResultRules.Any(x => x.Alias == "googlebot_none"));
        }
    }
}