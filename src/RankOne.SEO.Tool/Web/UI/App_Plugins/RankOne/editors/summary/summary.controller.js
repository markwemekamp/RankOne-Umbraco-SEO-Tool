(function () {

    // Controller
    function rankOne($scope, $http, editorState, resultService, webresultService) {

        $scope.sortOrder = ['-errorCount', '-warningCount', '-hintCount'];

        $scope.load = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?id={id}';
            webresultService.GetResult(editorState.current, url)
                .then(function(response) {
                        $scope.analyzeResults = response;
                        resultService.SetMetadata($scope.analyzeResults);
                        $scope.loading = false;
                    },
                    function(message) {
                        $scope.error = message;
                        $scope.loading = false;
                    });
        };

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOne);

})();