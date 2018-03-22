using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;

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
        public void IsMinified_OnExecuteUnminifiedCode_ReturnsFalse()
        {
            var input = @"<script>
                            alert('test');
                            </script>";



            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsFalse(minified);
        }

        [TestMethod]
        public void IsMinified_OnExecuteWithMinifiedCode_ReturnsTrue()
        {

            var minificationHelper = new MinificationHelper();
            var minified = minificationHelper.IsMinified(input);

            Assert.IsTrue(minified);
        }
    }
}
