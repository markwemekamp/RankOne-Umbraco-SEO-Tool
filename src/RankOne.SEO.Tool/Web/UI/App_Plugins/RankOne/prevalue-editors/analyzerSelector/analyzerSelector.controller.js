(function () {
    // Controller
    function analyzerSelectorController($scope, analyzeService) {
        analyzeService.GetStructure()
            .then(function (response) {
                $scope.analyzerSummaries = response;
                $scope.load();
                $scope.loading = false;
            },
            function (message) {
                $scope.error = message;
                $scope.loading = false;
            });

        $scope.load = function () {
            var tempObject = [];
            var firstTime = false;
            if (!$scope.model.value) {
                $scope.model.value = [];
                firstTime = true;
            }

            angular.forEach($scope.analyzerSummaries,
                function (analyzerSummary) {
                    var analyzerSummaryObject = _.findWhere($scope.model.value, { name: analyzerSummary.Name });

                    if (!analyzerSummaryObject) {
                        analyzerSummaryObject = {
                            name: analyzerSummary.Name,
                            checked: firstTime,
                            analyzers: []
                        };
                    }
                    angular.forEach(analyzerSummary.Analyzers,
                        function (analyzer) {
                            var analyzerObject = _.findWhere(analyzerSummaryObject.analyzers, { name: analyzer });

                            if (!analyzerObject) {
                                analyzerObject = {
                                    name: analyzer,
                                    checked: firstTime
                                }
                                analyzerSummaryObject.analyzers.push(analyzerObject);
                            }
                        });

                    tempObject.push(analyzerSummaryObject);
                });

            $scope.model.value = tempObject;
        }
    }

    // Register the controller
    angular.module("umbraco").controller('analyzerSelectorController', analyzerSelectorController);
})();