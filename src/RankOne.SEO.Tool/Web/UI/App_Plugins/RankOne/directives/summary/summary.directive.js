angular.module('umbraco').directive('summary', function (analyzeService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/summary/summary.directive.html',
        scope: {
            nodeId: '=',
            model: '='
        },
        link: function (scope) {
            scope.sortOrder = ['-ErrorCount', '-WarningCount', '-HintCount'];

            scope.load = function () {
                scope.loading = true;

                analyzeService.AnalyzeNode(scope.nodeId).then(function (response) {
                    scope.analyzeResults = response;
                    scope.loading = false;
                },
                    function (message) {
                        scope.error = message;
                        scope.loading = false;
                    });
            };

            scope.load();
        }
    }
});