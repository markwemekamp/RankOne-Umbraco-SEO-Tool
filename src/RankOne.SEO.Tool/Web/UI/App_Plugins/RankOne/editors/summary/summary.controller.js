(function () {

    // Controller
    function rankOne($scope, $http, $location, editorState, resultService) {

        $scope.sortOrder = ['-errorCount', '-warningCount', '-hintCount'];

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
                    resultService.SetMetadata($scope.analyzeResults);
                    $scope.loading = false;
                    console.log($scope.model);
                }, function errorCallback(response) {
                    console.log(response);
                });
            }
        }
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOne);

})();