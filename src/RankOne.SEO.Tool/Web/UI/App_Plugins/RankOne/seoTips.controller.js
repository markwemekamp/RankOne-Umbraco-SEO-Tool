(function () {

    // Controller
    function rankOne($scope, $http, $location, editorState, scoreService, resultService) {

        $scope.analyzeResults = null;
        $scope.filter = null;

        if (!editorState.current.template) {
            $scope.error = "This item does not have a template";
        } else {
            var url = editorState.current.urls[0];

            if (url == "This item is not published") {
                $scope.error = "This item is not published";
            } else {
                $scope.loading = true;
                url = $location.protocol() + "://" + $location.host() + ":" + $location.port() + url;

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
    angular.module("umbraco").controller('rankOne', rankOne);

})();