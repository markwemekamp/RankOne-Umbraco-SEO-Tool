using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using System;
using System.IO;
using System.Linq;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class ConfigurationHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullParameter_ThrowsException()
        {
            new ConfigurationHelper(null);
        }

        [TestMethod]
        public void ConfigFilePath_OnSet_SetsConfigFilePath()
        {
            var configurationHelper = new ConfigurationHelper
            {
                ConfigFilePath = "test"
            };
            Assert.AreEqual("test", configurationHelper.ConfigFilePath);
        }

        [TestMethod]
        public void ConfigFilePath_OnGet_ReturnsPath()
        {
            var configurationHelper = new ConfigurationHelper();
            var filepath = configurationHelper.ConfigFilePath;
            Assert.IsTrue(filepath.EndsWith("RankOne.Config"));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetSummaries_OnExecuteToNonExistingPath_ThrowsException()
        {
            var configurationHelper = new ConfigurationHelper();
            configurationHelper.GetSummaries();
        }

        [TestMethod]
        [DeploymentItem("../../files/RankOne.config", "files")]
        public void GetSummaries_OnExecute_ReadsConfigFile()
        {
            var configurationHelper = new ConfigurationHelper()
            {
                ConfigFilePath = "./files/RankOne.Config"
            };
            var summaries = configurationHelper.GetSummaries();

            Assert.IsNotNull(summaries);
            Assert.AreEqual(3, summaries.Count());
            Assert.AreEqual(7, summaries.ElementAt(0).Analyzers.Count());
            Assert.AreEqual(5, summaries.ElementAt(1).Analyzers.Count());
            Assert.AreEqual(5, summaries.ElementAt(2).Analyzers.Count());
        }
    }
}