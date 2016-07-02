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
                            $scope.setData(response.data);
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
            $scope.pageHierarchy = null;
            var url = '/umbraco/backoffice/api/RankOneApi/UpdateAllPages';

            $http({ method: 'GET', url: url })
                .then(function (response) {
                    if (response.data && response.status == 200) {
                        $scope.setData(response.data);
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
            $scope.pageHierarchy = null;
            var url = '/umbraco/backoffice/api/RankOneApi/Initialize';

            $http({ method: 'GET', url: url })
                .then(function (response) {
                    if (response.data && response.status == 200) {
                        $scope.setData(response.data);
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

        $scope.setData = function (data) {
            $('.dashboard-row').remove();
            $scope.pageHierarchy = data;

            var totalScore = 0;
            var items = 0;

            function addScore(item) {
                if (item.PageScore) {
                    totalScore += item.PageScore.OverallScore;
                    items++;
                }
                _.each(item.Children, addScore);
            }
            _.each(data, addScore);

            $scope.overallScore = Math.round(totalScore / items);

            $scope.color = '#4db53c';
            if ($scope.overallScore < 33) {
                $scope.color = '#dd2222';
            } else if ($scope.overallScore < 66) {
                $scope.color = '#dd9d22';
            }

            $("#score")
                .circliful({
                    percent: $scope.overallScore,
                    fontColor: '#000000',
                    foregroundColor: $scope.color,
                    percentageTextSize: 30
                });
        };

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSiteDashboard', rankOneSiteDashboard);

})();