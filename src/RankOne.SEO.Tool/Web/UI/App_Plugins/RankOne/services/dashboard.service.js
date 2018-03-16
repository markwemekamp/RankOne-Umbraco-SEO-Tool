angular.module('umbraco')
    .service('dashboardService',
    function (webresultService, umbRequestHelper) {
        this.GetPageHierarchy = function () {
            var url = umbRequestHelper.getApiUrl("DashboardApi", "GetPageHierarchy");
            return webresultService.GetResult(url);
        },
        this.UpdateAllPages = function () {
            var url = umbRequestHelper.getApiUrl("DashboardApi", "UpdateAllPages");
            return webresultService.GetResult(url);
        },
        this.Initialize = function () {
            var url = umbRequestHelper.getApiUrl("DashboardApi", "Initialize");
            return webresultService.GetResult(url);
        };
});