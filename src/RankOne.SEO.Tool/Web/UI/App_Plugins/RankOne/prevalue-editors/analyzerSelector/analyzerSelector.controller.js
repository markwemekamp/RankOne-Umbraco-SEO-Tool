(function () {

    // Controller
    function analyzerSelectorController($scope) {

        $scope.analyzerSummaries = [
        {
            name: 'htmlanalyzer',
            analyzers: [
                'titleanalyzer',
                'metadescriptionanalyzer',
                'imagetaganalyzer',
                'deprecatedtaganalyzer',
                'headinganalyzer'
            ]
        },
        {
            name: 'keywordanalyzer',
            analyzers: [
                'keywordtitleanalyzer',
                'keywordmetadescriptionanalyzer'
            ]
        },
        {
            name: 'speedanalyzer',
            analyzers: [
                'serverresponseanalyzer',
                'gzipanalyzer',
                'htmlsizeanalyzer',
                'additionalcallanalyzer'
            ]
        },
        ];

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        if ($scope.model.value.length == 0) {
            angular.forEach($scope.analyzerSummaries, function (analyzerSummary) {
                var analyzerSummaryObject = {
                    name: analyzerSummary.name,
                    analyzers: [],
                    checked: true
                };

                angular.forEach(analyzerSummary.analyzers, function (analyzer) {
                    analyzerSummaryObject.analyzers.push({
                        name: analyzer,
                        checked: true
                    });
                });

                $scope.model.value.push(analyzerSummaryObject);
            });
        }
    };

    // Register the controller
    angular.module("umbraco").controller('analyzerSelectorController', analyzerSelectorController);
})();
