angular.module('umbraco')
    .service('dashboardService',
        function (webresultService) {
            this.GetPageHierarchy = function () {
                var url = '/umbraco/backoffice/rankone/RankOneApi/GetPageHierarchy';
                return webresultService.GetResult(url);
            },
            this.UpdateAllPages = function () {
                var url = '/umbraco/backoffice/rankone/RankOneApi/UpdateAllPages';
                return webresultService.GetResult(url);
            },
            this.Initialize = function () {
                var url = '/umbraco/backoffice/rankone/RankOneApi/Initialize';
                return webresultService.GetResult(url);
            };
        });