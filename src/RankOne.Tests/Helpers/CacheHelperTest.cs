using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;
using System.Collections.Generic;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class CacheHelperTest
    {
        private CacheHelper _cacheHelper;

        [TestInitialize]
        public void TestInit()
        {
            _cacheHelper = new CacheHelper();
            _cacheHelper.SetValue("existing", "I exist");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exists_OnExecuteWithNullParameter_ThrowsException()
        {
            _cacheHelper.Exists(null);
        }

        [TestMethod]
        public void Exists_OnExecuteWithRandomKey_ReturnsFalse()
        {
            var exists = _cacheHelper.Exists("random");
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void Exists_OnExecuteWithExistingKey_ReturnsTrue()
        {
            var exists = _cacheHelper.Exists("existing");
            Assert.IsTrue(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValue_OnExecuteWithNullParameter_ThrowsException()
        {
            _cacheHelper.GetValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetValue_OnExecuteWithRandomKey_ThrowsKeyNotFoundException()
        {
            _cacheHelper.GetValue("random");
        }

        [TestMethod]
        public void GetValue_OnExecuteWithExistingKey_ReturnsValue()
        {
            var value = _cacheHelper.GetValue("existing");
            Assert.IsNotNull(value);
            Assert.AreEqual("I exist", value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetValue_OnExecuteWithNullParameter_ThrowsException()
        {
            _cacheHelper.SetValue(null, null);
        }
    }
}
