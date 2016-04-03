(function () {

    // Controller
    function rankOne($scope, $http, $location, editorState, scoreService, resultService) {

        $scope.analyzeResults = null;
        $scope.loading = true;
        

        if (!editorState.current.template) {
            // geen template gekoppeld
        } else {
            var url = editorState.current.urls[0];

            if (url == "This item is not published") {
                // item niet gepublished

                // TODO preview fallback
            } else {
                // validatie van url

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
                    $scope.succesCount = scoreService.getTotalSuccesCount(results);

                    $scope.loading = false;

                }, function errorCallback(response) {
                    console.log(response);
                });
            }
        }

    };

    // Register the controller
    angular.module("umbraco").controller('rankOne', rankOne);

})();