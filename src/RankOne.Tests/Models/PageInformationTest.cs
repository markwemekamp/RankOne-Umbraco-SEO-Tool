using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class PageInformationTest
    {
        [TestMethod]
        public void TitleProperty_OnSet_SetsTheValue()
        {
            var pageInformation = new PageInformation();
            pageInformation.Title = "title";

            Assert.AreEqual("title", pageInformation.Title);
        }

        [TestMethod]
        public void DescriptionProperty_OnSet_SetsTheValue()
        {
            var pageInformation = new PageInformation();
            pageInformation.Description = "description";

            Assert.AreEqual("description", pageInformation.Description);
        }

        [TestMethod]
        public void UrlProperty_OnSet_SetsTheValue()
        {
            var pageInformation = new PageInformation();
            pageInformation.Url = "url";

            Assert.AreEqual("url", pageInformation.Url);
        }
    }
}
