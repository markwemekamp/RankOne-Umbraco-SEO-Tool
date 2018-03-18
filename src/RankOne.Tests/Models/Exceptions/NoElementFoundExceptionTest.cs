using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models.Exceptions;
using System;
namespace RankOne.Tests.Models.Exceptions
{
    [TestClass]
    public class NoElementFoundExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowException()
        {
            new NoElementFoundException(null);
        }

        [TestMethod]
        public void Constructor_OnExecute_SetsElementParameter()
        {
            var exception = new NoElementFoundException("div");
            var result = exception.ElementName;

            Assert.AreEqual("div", result);
        }
    }
}
