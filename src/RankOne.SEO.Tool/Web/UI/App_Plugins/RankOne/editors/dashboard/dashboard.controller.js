(function () {

    // Controller
    function rankOne($scope, $http, editorState, scoreService, resultService, urlService, localizationService) {

        $scope.analyzeResults = null;
        $scope.filter = null;

        if (!editorState.current.template) {
            $scope.error = localizationService.localize("error_no_template");
        } else {
            var relativeUrl = editorState.current.urls[0];

            if (relativeUrl == "This item is not published") {
                $scope.error = localizationService.localize("error_not_published");
            } else {
                $scope.loading = true;
                var url = urlService.GetUrl(relativeUrl);

                $http({
                    method: 'GET',
                    url: '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?url=' + url
                }).then(function successCallback(response) {
                    $scope.analyzeResults = response.data;

                    var results = resultService.SetMetadata($scope.analyzeResults);

                    $scope.overallScore = scoreService.getOverallScore(results);
                    $scope.errorCount = scoreService.getTotalErrorCount(results);
                    $scope.warningCount = scoreService.getTotalWarningCount(results);
                    $scope.hintCount = scoreService.getTotalHintCount(results);
                    $scope.successCount = scoreService.getTotalSuccessCount(results);

                    $scope.loading = false;

                }, function errorCallback(response) {
                    console.log(response);
                });
            }
        }

        $scope.setFilter = function (filter) {
            $scope.filter = filter;
        };
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneDashboard', rankOne);

})();