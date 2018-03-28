using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;
using System.IO;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class MinificationHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsMinified_OnExecuteWithNullParameter_ThrowsException()
        {
            var minificationHelper = new MinificationHelper();
            minificationHelper.IsMinified(null);
        }

        [TestMethod]
        [DeploymentItem("../../files/unminified.js", "files")]
        public void IsMinified_OnExecuteUnminifiedJs_ReturnsFalse()
        {
            var input = File.ReadAllText("./files/unminified.js");
            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsFalse(minified);
        }

        [TestMethod]
        [DeploymentItem("../../files/minified.js", "files")]
        public void IsMinified_OnExecuteWithMinifiedJs_ReturnsTrue()
        {
            var input = File.ReadAllText("./files/minified.js");
            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsTrue(minified);
        }

        [TestMethod]
        [DeploymentItem("../../files/unminified.css", "files")]
        public void IsMinified_OnExecuteUnminifiedCss_ReturnsFalse()
        {
            var input = File.ReadAllText("./files/unminified.css");
            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsFalse(minified);
        }

        [TestMethod]
        [DeploymentItem("../../files/minified.css", "files")]
        public void IsMinified_OnExecuteWithMinifiedCss_ReturnsTrue()
        {
            var input = File.ReadAllText("./files/minified.css");
            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsTrue(minified);
        }
    }
}