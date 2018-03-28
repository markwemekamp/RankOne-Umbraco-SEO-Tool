using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Controllers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class DashboardApiControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForAnalyzeService_ThrowsException()
        {
            new DashboardApiController((IDashboardDataService)null);
        }
    }
}
