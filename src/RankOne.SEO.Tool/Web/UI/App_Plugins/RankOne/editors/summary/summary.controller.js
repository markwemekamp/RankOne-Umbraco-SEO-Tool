(function () {

    // Controller
    function rankOne($scope, $http, editorState, resultService, urlService, localizationService) {

        $scope.sortOrder = ['-errorCount', '-warningCount', '-hintCount'];

        $scope.load = function () {
            $scope.loading = true;
            if (!editorState.current.template) {
                $scope.error = localizationService.localize("error_no_template");
                $scope.loading = false;
            } else {
                var relativeUrl = editorState.current.urls[0];

                if (relativeUrl == "This item is not published") {
                    $scope.error = localizationService.localize("error_not_published");
                    $scope.loading = false;
                } else {
                    $scope.loading = true;
                    var url = urlService.GetUrl(relativeUrl);

                    $http({
                        method: 'GET',
                        url: '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?url=' + url
                    }).then(function successCallback(response) {
                        $scope.analyzeResults = response.data;
                        resultService.SetMetadata($scope.analyzeResults);
                        $scope.loading = false;
                    }, function errorCallback(response) {
                        $scope.error = response;
                        $scope.loading = false;
                    });
                }
            }
        }

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOne);

})();