angular.module('umbraco')
    .service('dashboardService',
        function (webresultService) {
            this.GetPageHierarchy = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/GetPageHierarchy';
                return webresultService.GetResult(url);
            },
            this.UpdateAllPages = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/UpdateAllPages';
                return webresultService.GetResult(url);
            },
            this.Initialize = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/Initialize';
                return webresultService.GetResult(url);
            };
        });