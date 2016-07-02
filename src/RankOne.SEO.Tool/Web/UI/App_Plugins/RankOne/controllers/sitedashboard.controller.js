(function () {

    // Controller
    function rankOneSiteDashboard($scope, $http, webresultService, localizationService) {

        $scope.analyzeResults = null;
        $scope.filter = null;
        $scope.initialized = false;

        $scope.load = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/api/RankOneApi/GetPageHierarchy';

            $http({ method: 'GET', url: url })
                .then(function (response) {
                    if (response.data && response.status == 200) {                        
                        if (response.data == null || response.data == "null") {
                            $scope.initialized = false;
                        } else {
                            $scope.pageHierarchy = response.data;
                            $scope.initialized = true;
                        }
                        $scope.loading = false;
                    } else {
                        $scope.error = localizationService.localize("error_page_error", [response.status]);
                        $scope.loading = false;
                    }
                },
                function (response) {
                    $scope.error = response.data.Message;
                    $scope.loading = false;
                });
        };

        $scope.refresh = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/api/RankOneApi/UpdateAllPages';

            $http({ method: 'GET', url: url })
                .then(function (response) {
                    if (response.data && response.status == 200) {
                        $scope.pageHierarchy = response.data;
                        $scope.loading = false;
                    } else {
                        $scope.error = localizationService.localize("error_page_error", [response.status]);
                        $scope.loading = false;
                    }
                },
                function (response) {
                    $scope.error = response.data.Message;
                    $scope.loading = false;
                });
        };

        $scope.initialize = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/api/RankOneApi/Initialize';

            $http({ method: 'GET', url: url })
                .then(function (response) {
                    if (response.data && response.status == 200) {
                        $scope.pageHierarchy = response.data;
                        $scope.initialized = true;
                        $scope.loading = false;
                    } else {
                        $scope.initialized = true;
                        $scope.error = localizationService.localize("error_page_error", [response.status]);
                        $scope.loading = false;
                    }
                },
                function (response) {
                    $scope.error = response.data.Message;
                    $scope.loading = false;
                });
        };

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSiteDashboard', rankOneSiteDashboard);

})();