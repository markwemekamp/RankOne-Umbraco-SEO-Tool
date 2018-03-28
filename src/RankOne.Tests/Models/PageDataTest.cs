using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class PageDataTest
    {
        [TestMethod]
        public void DocumentProperty_OnSet_SetsTheValue()
        {
            var pageData = new PageData();
            var doc = new HtmlDocument();
            doc.LoadHtml("<div></div>");
            pageData.Document = doc.DocumentNode;
            Assert.IsNotNull(pageData.Document);
        }

        [TestMethod]
        public void FocuskeywordProperty_OnSet_SetsTheValue()
        {
            var pageData = new PageData();
            pageData.Focuskeyword = "focus";
            Assert.AreEqual("focus", pageData.Focuskeyword);
        }

        [TestMethod]
        public void UrlProperty_OnSet_SetsTheValue()
        {
            var pageData = new PageData();
            pageData.Url = "url";
            Assert.AreEqual("url", pageData.Url);
        }
    }
}