using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class ConfigurationHelperTest
    {
        private ConfigurationHelper _configurationHelper;

        [TestInitialize]
        public void TestInit()
        {
            _configurationHelper = new ConfigurationHelper();
        }

        [TestMethod]
        public void ConfigFileName_OnGet_ReturnsDefaultValue()
        {
            var filename = _configurationHelper.ConfigFileName;
            Assert.AreEqual("RankOne.Config", filename);
        }

        [TestMethod]
        public void ConfigFilePath_OnGet_ReturnsPath()
        {
            var filepath = _configurationHelper.ConfigFilePath;
            Assert.IsTrue(filepath.EndsWith("RankOne.Config"));
        }


    }
}
