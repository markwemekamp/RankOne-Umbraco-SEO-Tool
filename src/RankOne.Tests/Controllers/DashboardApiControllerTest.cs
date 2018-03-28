using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RankOne.Controllers;
using RankOne.Interfaces;
using System;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class DashboardApiControllerTest : BaseUmbracoControllerTest
    {
        private Mock<IDashboardDataService> _dashboardDataServiceMock;
        private DashboardApiController _controllerMock;

        [TestInitialize]
        public void Initialize()
        {
            _dashboardDataServiceMock = new Mock<IDashboardDataService>();

            _controllerMock = new DashboardApiController(_dashboardDataServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNullForAnalyzeService_ThrowsException()
        {
            new DashboardApiController((IDashboardDataService)null);
        }

        [TestMethod]
        public void Initialize_OnExecute_ReturnsOk()
        {
            var result = _controllerMock.Initialize();

            _dashboardDataServiceMock.Verify(x => x.Initialize());
            _dashboardDataServiceMock.Verify(x => x.GetUpdatedHierarchy());
        }

        [TestMethod]
        public void GetPageHierarchy_OnExecute_ReturnsOk()
        {
            var result = _controllerMock.GetPageHierarchy();

            _dashboardDataServiceMock.Verify(x => x.GetHierarchyFromCache());
        }

        [TestMethod]
        public void UpdateAllPages_OnExecute_ReturnsOk()
        {
            var result = _controllerMock.UpdateAllPages();

            _dashboardDataServiceMock.Verify(x => x.GetUpdatedHierarchy());
        }
    }
}