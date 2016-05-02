(function () {

    // Controller
    function analyzerSelectorController($scope) {

        $scope.analyzerSummaries = [
        {
            name: 'htmlanalyzer',
            analyzers: [
                'titleanalyzer',
                'metadescriptionanalyzer',
                'metakeywordanalyzer',
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
                'additionalcallanalyzer',
                'cssminificationanalyzer'
            ]
        }
        ];

        var firstTime = false;
        if (!$scope.model.value) {
            $scope.model.value = [];
            firstTime = true;
        }

        angular.forEach($scope.analyzerSummaries, function (analyzerSummary) {

            var analyzerSummaryObject = _.findWhere($scope.model.value, { name: analyzerSummary.name });

            if (!analyzerSummaryObject) {
                analyzerSummaryObject = {
                    name: analyzerSummary.name,
                    checked: firstTime
                };

                angular.forEach(analyzerSummary.analyzers, function (analyzer) {
                    var analyzerObject = {
                        name: analyzer,
                        checked: firstTime
                    };
                    analyzerSummaryObject.analyzers.push(analyzerObject);
                });

                $scope.model.value.push(analyzerSummaryObject);
            } else {
                angular.forEach(analyzerSummary.analyzers, function (analyzer) {

                    var analyzerObject = _.findWhere(analyzerSummaryObject.analyzers, { name: analyzer });

                    if (!analyzerObject) {
                        analyzerObject = {
                            name: analyzer,
                            checked: firstTime
                        }
                        analyzerSummaryObject.analyzers.push(analyzerObject);
                    }
                });
            }
        });
    };

    // Register the controller
    angular.module("umbraco").controller('analyzerSelectorController', analyzerSelectorController);
})();
