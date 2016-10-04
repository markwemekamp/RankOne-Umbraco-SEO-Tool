(function () {

    // Controller
    function rankOneSiteDashboard($scope, webresultService) {

        $scope.analyzeResults = null;
        $scope.filter = null;
        $scope.initialized = false;

        $scope.load = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/rankone/RankOneApi/GetPageHierarchy';

            webresultService.GetResult(url).then(function (response) {
                $scope.setData(response);
                $scope.initialized = true;
                $scope.loading = false;
            },
            function (response) {
                $scope.error = response;
                $scope.loading = false;
            });
        };

        $scope.refresh = function () {
            $scope.loading = true;
            $scope.pageHierarchy = null;
            var url = '/umbraco/backoffice/rankone/RankOneApi/UpdateAllPages';

            webresultService.GetResult(url).then(function (response) {
                $scope.setData(response);
                $scope.initialized = true;
                $scope.loading = false;
            },
            function (response) {
                $scope.error = response;
                $scope.loading = false;
            });
        };

        $scope.initialize = function () {
            $scope.loading = true;
            $scope.pageHierarchy = null;
            var url = '/umbraco/backoffice/rankone/RankOneApi/Initialize';

            webresultService.GetResult(url).then(function (response) {
                $scope.setData(response);
                $scope.initialized = true;
                $scope.loading = false;
            },
           function (response) {
               $scope.error = response;
               $scope.loading = false;
           });
        };

        $scope.setData = function (data) {

            // Remove the previously added rows from dashboardrow directive
            $('.dashboard-row').remove();
            $scope.pageHierarchy = data;

            var totalScore = 0;
            var items = 0;

            // sum the overallscore per node
            function addScore(item) {
                if (item.PageScore) {
                    totalScore += item.PageScore.OverallScore;
                    items++;
                }
                _.each(item.Children, addScore);
            }
            _.each(data, addScore);

            if (items > 0) {
                $scope.overallScore = Math.round(totalScore / items);
            } else {
                $scope.overallScore = 0;
            }

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