using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using RankOne.Models.Exceptions;
using System;
using System.Linq;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class HtmlTagHelperTest
    {
        [TestMethod]
        public void GetHeadTag_OnExecuteWithHeadTag_ReturnsTheHeadNode()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><head><title>Title</title></head></html>");

            var htmlTagHelper = new HtmlTagHelper();
            var headTag = htmlTagHelper.GetHeadTag(doc.DocumentNode);

            Assert.IsNotNull(headTag);
            Assert.AreEqual("<title>Title</title>", headTag.InnerHtml);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetHeadTag_OnExecuteWithNullParameter_ThrowsException()
        {
            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetHeadTag(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoElementFoundException))]
        public void GetHeadTag_OnExecuteWithNoHeadTag_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><title>Title</title></html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetHeadTag(doc.DocumentNode);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleElementsFoundException))]
        public void GetHeadTag_OnExecuteWithMultipleHeadTags_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><head><title>Title</title></head><head><title>Title</title></head></html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetHeadTag(doc.DocumentNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBodyTag_OnExecuteWithNullParameter_ThrowsException()
        {
            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetBodyTag(null);
        }

        [TestMethod]
        public void GetBodyTag_OnExecuteWithBodyTag_ReturnsTheBodyNode()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><body><div>Content</div></body></html>");

            var htmlTagHelper = new HtmlTagHelper();
            var bodyTag = htmlTagHelper.GetBodyTag(doc.DocumentNode);

            Assert.IsNotNull(bodyTag);
            Assert.AreEqual("<div>Content</div>", bodyTag.InnerHtml);
        }

        [TestMethod]
        [ExpectedException(typeof(NoElementFoundException))]
        public void GetBodyTag_OnExecuteWithNoBodyTag_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><title>Title</title></html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetBodyTag(doc.DocumentNode);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleElementsFoundException))]
        public void GetBodyTag_OnExecuteWithMultipleBodyTags_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><body><div>Content</div></body><body><div>Content</div></body></html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetBodyTag(doc.DocumentNode);
        }

        [TestMethod]
        public void GetTitleTag_OnExecuteWithHeadTag_ReturnsTheTitleNode()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><head><title>Title</title></head></html>");

            var htmlTagHelper = new HtmlTagHelper();
            var titleTag = htmlTagHelper.GetTitleTag(doc.DocumentNode);

            Assert.IsNotNull(titleTag);
            Assert.AreEqual("Title", titleTag.InnerHtml);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTitleTag_OnExecuteWithNullParameter_ThrowsException()
        {
            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetTitleTag(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoElementFoundException))]
        public void GetTitleTag_OnExecuteWithNoTitleTag_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html>Title</html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetTitleTag(doc.DocumentNode);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleElementsFoundException))]
        public void GetTitleTag_OnExecuteWithMultipleTitleTags_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><head><title>Title</title></head><head><title>Title</title></head></html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetTitleTag(doc.DocumentNode);
        }

        [TestMethod]
        public void GetMetaTags_OnExecuteWithMetaTags_ReturnsMetaTagsNodes()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html><meta /><meta /></html>");

            var htmlTagHelper = new HtmlTagHelper();
            var metaTags = htmlTagHelper.GetMetaTags(doc.DocumentNode);

            Assert.IsNotNull(metaTags);
            Assert.AreEqual(2, metaTags.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMetaTags_OnExecuteWithNullParameter_ThrowsException()
        {
            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetMetaTags(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoElementFoundException))]
        public void GetMetaTags_OnExecuteWithNoMetaTags_ThrowsException()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html>Empty</html>");

            var htmlTagHelper = new HtmlTagHelper();
            htmlTagHelper.GetMetaTags(doc.DocumentNode);
        }
    }
}
