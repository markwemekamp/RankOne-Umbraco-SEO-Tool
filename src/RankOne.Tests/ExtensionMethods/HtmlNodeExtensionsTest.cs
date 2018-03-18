using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.ExtensionMethods;
using System;
using System.Linq;

namespace RankOne.Tests.ExtensionMethods
{
    [TestClass]
    public class HtmlNodeExtensionsTest
    {
        private HtmlNode _htmlNode;

        [TestInitialize]
        public void TestInit()
        {
            _htmlNode = HtmlNode.CreateNode(@"<div id=""test"" ref=""ref"">
                                                <p>test</p>
                                                <div id=""div"">
                                                    <p id=""p"">test 2</p>
                                                    <span id=""span""><p>test 3</p></span>
                                                </div>
                                            </div>");
        }

        #region Null parameter checks

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetElements_OnExecuteWithNullParameter_ThrowsException()
        {
            _htmlNode.GetElements(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetElementsWithAttribute_OnExecuteWithNullElementParameter_ThrowsException()
        {
            _htmlNode.GetElementsWithAttribute(null, "param");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetElementsWithAttribute_OnExecuteWithNullAttributeParameter_ThrowsException()
        {
            _htmlNode.GetElementsWithAttribute("elem", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAttribute_OnExecuteWithNullParameter_ThrowsException()
        {
            _htmlNode.GetAttribute(null);
        }

        #endregion Null parameter checks

        [TestMethod]
        public void GetElements_OnExecuteWithSpanParameter_Returns2Objects()
        {
            var result = _htmlNode.GetElements("span");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void GetElements_OnExecuteWithParagraphParameter_Returns2Objects()
        {
            var result = _htmlNode.GetElements("p");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public void GetElements_OnExecuteWithDivParameter_Returns2Objects()
        {
            var result = _htmlNode.GetElements("div");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void GetElements_OnExecuteWithMarqueeParameter_Returns0Objects()
        {
            var result = _htmlNode.GetElements("marquee");

            Assert.IsNotNull(result);
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void GetElementsWithAttribute_OnExecuteWithParagraphAndIdParameter_Returns1Object()
        {
            var result = _htmlNode.GetElementsWithAttribute("p", "id");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void GetElementsWithAttribute_OnExecuteWithParagraphAndClassParameter_Returns0Object()
        {
            var result = _htmlNode.GetElementsWithAttribute("p", "class");

            Assert.IsNotNull(result);
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void GetAttribute_OnExecuteWithIdParameter_ReturnsAttribute()
        {
            var result = _htmlNode.GetAttribute("id");

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Value);
        }

        [TestMethod]
        public void GetAttribute_OnExecuteWithClassParameter_ReturnsNull()
        {
            var result = _htmlNode.GetAttribute("class");

            Assert.IsNull(result);
        }
    }
}