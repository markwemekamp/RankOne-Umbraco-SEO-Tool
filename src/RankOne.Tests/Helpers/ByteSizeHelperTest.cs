using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class ByteSizeHelperTest
    {
        private ByteSizeHelper _byteSizeHelper;

        [TestInitialize]
        public void TestInit()
        {
            _byteSizeHelper = new ByteSizeHelper();
        }

        [TestMethod]
        public void GetByteSize_OnExecuteWithNullParameter_Returns0()
        {
            var size = _byteSizeHelper.GetByteSize(null);
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void GetByteSize_OnExecuteWith4LetterWord_Returns4()
        {
            var size = _byteSizeHelper.GetByteSize("test");
            Assert.AreEqual(4, size);
        }

        [TestMethod]
        public void GetByteSize_OnExecuteWith8LetterWord_Returns8()
        {
            var size = _byteSizeHelper.GetByteSize("testtest");
            Assert.AreEqual(8, size);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue0_Returns0Bytes()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(0);
            Assert.AreEqual("0 bytes", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue1023_Returns1023Bytes()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(1023);
            Assert.AreEqual("1023 bytes", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue1024_Returns1KB()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(1024);
            Assert.AreEqual("1 KB", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue2047_Returns1KB()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(2047);
            Assert.AreEqual("1 KB", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue2048_Returns2KB()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(2048);
            Assert.AreEqual("2 KB", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValue1048576_Returns1MB()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(1048576);
            Assert.AreEqual("1 MB", suffix);
        }

        [TestMethod]
        public void GetSizeSuffix_OnExecuteWithValueMinus1024_ReturnsMinus1KB()
        {
            var suffix = _byteSizeHelper.GetSizeSuffix(-1024);
            Assert.AreEqual("-1 KB", suffix);
        }
    }
}
