using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class HtmlResultTest
    {
        [TestMethod]
        public void HtmlProperty_OnSet_SetsTheValue()
        {
            var htmlResult = new HtmlResult();
            htmlResult.Html = "html";
            Assert.AreEqual("html", htmlResult.Html);
        }

        [TestMethod]
        public void HtmlProperty_OnSet_SetsTheValueToNull()
        {
            var htmlResult = new HtmlResult();
            htmlResult.Html = "html";
            htmlResult.Html = null;
            Assert.IsNull(htmlResult.Html);
        }

        [TestMethod]
        public void DocumentProperty_OnSet_SetsTheValue()
        {
            var htmlResult = new HtmlResult();
            var doc = new HtmlDocument();
            doc.LoadHtml("<div></div>");
            htmlResult.Document = doc.DocumentNode;
            Assert.AreEqual("<div></div>", htmlResult.Document.InnerHtml);
        }

        [TestMethod]
        public void DocumentProperty_OnSet_SetsTheValueToNull()
        {
            var htmlResult = new HtmlResult();
            var doc = new HtmlDocument();
            doc.LoadHtml("<div></div>");
            htmlResult.Document = doc.DocumentNode;
            htmlResult.Document = null;
            Assert.IsNull(htmlResult.Document);
        }
    }
}
