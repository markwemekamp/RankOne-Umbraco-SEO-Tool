using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class EncodingHelperTest
    {
        private EncodingHelper _encodingHelper;

        [TestInitialize]
        public void TestInit()
        {
            _encodingHelper = new EncodingHelper();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEncodingByUrl_OnExecuteWithNullParameter_ThrowsException()
        {
            _encodingHelper.GetEncodingByUrl(null);
        }
    }
}