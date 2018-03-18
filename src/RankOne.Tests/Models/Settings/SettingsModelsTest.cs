using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankOne.Tests.Models.Settings
{
    [TestClass]
    public class SettingsModelsTest
    {
        [TestMethod]
        public void AnalyzerOptionKeyProperty_GetSet()
        {
            var instance = new AnalyzerOption();
            instance.Key = "key";
            Assert.AreEqual("key", instance.Key);
        }

        [TestMethod]
        public void AnalyzerOptionValueProperty_GetSet()
        {
            var instance = new AnalyzerOption();
            instance.Value = "value";
            Assert.AreEqual("value", instance.Value);
        }

        [TestMethod]
        public void AnalyzerSettingsAliasProperty_GetSet()
        {
            var instance = new AnalyzerSettings();
            instance.Alias = "alias";
            Assert.AreEqual("alias", instance.Alias);
        }

        [TestMethod]
        public void AnalyzerSettingsTypeProperty_GetSet()
        {
            var instance = new AnalyzerSettings();
            instance.Type = "type";
            Assert.AreEqual("type", instance.Type);
        }

        [TestMethod]
        public void AnalyzerSettingsWeightProperty_GetSet()
        {
            var instance = new AnalyzerSettings();
            instance.Weight = 3;
            Assert.AreEqual(3, instance.Weight);
        }

        [TestMethod]
        public void AnalyzerSettingsWeightValueProperty_GetSet()
        {
            var instance = new AnalyzerSettings();
            instance.Weight = 3;
            Assert.AreEqual("3", instance.WeightValue);
            instance.WeightValue = "10";
            Assert.AreEqual(10, instance.Weight);
            instance.Weight = null;
            Assert.IsNull(instance.WeightValue);
            instance.WeightValue = "test";
            Assert.AreEqual(0, instance.Weight);
        }

        [TestMethod]
        public void AnalyzerSettingsOptionsProperty_GetSet()
        {
            var instance = new AnalyzerSettings();
            instance.Options = new List<AnalyzerOption>();
            Assert.IsNotNull(instance.Options);
        }

        [TestMethod]
        public void RankOneSettingsSummariesProperty_GetSet()
        {
            var instance = new RankOneSettings();
            instance.Summaries = new List<SummarySettings>();
            Assert.IsNotNull(instance.Summaries);
        }

        [TestMethod]
        public void SummarySettingsNameProperty_GetSet()
        {
            var instance = new SummarySettings();
            instance.Name = "name";
            Assert.AreEqual("name", instance.Name);
        }

        [TestMethod]
        public void SummarySettingsAliasProperty_GetSet()
        {
            var instance = new SummarySettings();
            instance.Alias = "alias";
            Assert.AreEqual("alias", instance.Alias);
        }

        [TestMethod]
        public void SummarySettingsTypeProperty_GetSet()
        {
            var instance = new SummarySettings();
            instance.Type = "type";
            Assert.AreEqual("type", instance.Type);
        }

        [TestMethod]
        public void SummarySettingsSummariesProperty_GetSet()
        {
            var instance = new SummarySettings();
            instance.Analyzers = new List<AnalyzerSettings>();
            Assert.IsNotNull(instance.Analyzers);
        }
    }
}
