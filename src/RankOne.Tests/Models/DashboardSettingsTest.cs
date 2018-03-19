using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    public class DashboardSettingsTest
    {
        [TestMethod]
        public void FocusKeywordProperty_OnSet_SetsTheValue()
        {
            var dashboardSettings = new DashboardSettings();
            dashboardSettings.FocusKeyword = "focus";
            Assert.AreEqual("focus", dashboardSettings.FocusKeyword);
        }

        [TestMethod]
        public void FocusKeywordProperty_OnSet_SetsTheValueToNull()
        {
            var dashboardSettings = new DashboardSettings();
            dashboardSettings.FocusKeyword = null;
            Assert.IsNull(dashboardSettings.FocusKeyword);
        }
    }
}
