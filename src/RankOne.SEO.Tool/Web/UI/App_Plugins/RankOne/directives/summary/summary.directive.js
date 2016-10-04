angular.module('umbraco').directive('summary', function (webresultService) {
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

                var url = '/umbraco/backoffice/rankone/RankOneApi/AnalyzeNode?id=' + scope.nodeId;
                webresultService.GetResult(url)
                    .then(function (response) {
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