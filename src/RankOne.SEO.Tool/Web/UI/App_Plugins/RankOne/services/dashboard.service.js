angular.module('umbraco')
    .service('dashboardService',
    function (webresultService, rankOneRequestHelper) {
        this.GetPageHierarchy = function () {
            var url = rankOneRequestHelper.GetApiUrl("DashboardApi", "GetPageHierarchy");
            return webresultService.GetResult(url);
        }
        this.UpdateAllPages = function () {
            var url = rankOneRequestHelper.GetApiUrl("DashboardApi", "UpdateAllPages");
            return webresultService.GetResult(url);
        }
        this.Initialize = function () {
            var url = rankOneRequestHelper.GetApiUrl("DashboardApi", "Initialize");
            return webresultService.GetResult(url);
        }
    });