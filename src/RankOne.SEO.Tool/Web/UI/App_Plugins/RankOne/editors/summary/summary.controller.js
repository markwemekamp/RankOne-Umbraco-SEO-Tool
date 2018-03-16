(function () {
    // Controller
    function rankOneSummary($scope, editorState, analyzeService, localizationService) {
        $scope.load = function () {
            $scope.loading = true;

            if (!editorState.current.published) {
                $scope.error = localizationService.localize("error_not_published");
                $scope.loading = false;
            } else {
                analyzeService.AnalyzeNodeForEditor(editorState.current)
                    .then(function (response) {
                        $scope.analyzeResults = response;
                        $scope.loading = false;
                    },
                        function (message) {
                            $scope.error = message;
                            $scope.loading = false;
                        });
            }
        };

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOneSummary);
})();