using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class OptionTest
    {
        [TestMethod]
        public void KeyProperty_OnSet_SetsTheValue()
        {
            var option = new Option();
            option.Key = "key";

            Assert.AreEqual("key", option.Key);
        }

        [TestMethod]
        public void ValueProperty_OnSet_SetsTheValue()
        {
            var option = new Option();
            option.Value = "value";

            Assert.AreEqual("value", option.Value);
        }
    }
}
