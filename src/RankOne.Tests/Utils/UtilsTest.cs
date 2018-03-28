using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RankOne.Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateString_OnExecuteWith0_ThrowsException()
        {
            Utils.GenerateString(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateString_OnExecuteWithMinusNumber_ThrowsException()
        {
            Utils.GenerateString(-1);
        }

        [TestMethod]
        public void GenerateString_OnExecuteWith1_ReturnsAStringWithLength1()
        {
            var result = Utils.GenerateString(1);
            Assert.AreEqual(1, result.Length);
        }

        [TestMethod]
        public void GenerateString_OnExecuteWith100_ReturnsAStringWithLength100()
        {
            var result = Utils.GenerateString(100);
            Assert.AreEqual(100, result.Length);
        }

        [TestMethod]
        public void GenerateString_OnExecuteWith10000_ReturnsAStringWithLength10000()
        {
            var result = Utils.GenerateString(10000);
            Assert.AreEqual(10000, result.Length);
        }
    }
}