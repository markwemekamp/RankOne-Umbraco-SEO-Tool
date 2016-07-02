angular.module('umbraco').directive('accordion', function ($filter) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/accordion/accordion.directive.html',
        transclude: true,
        scope: {
            result: '=',
            filter: '='
        },
        link: function (scope) {

            scope.$watch('filter', function () {
                scope.expanded = scope.result && ((scope.filter == 'error' && scope.result.ErrorCount > 0) ||
                    (scope.filter == 'warning' && scope.result.WarningCount > 0) ||
                    (scope.filter == 'hint' && scope.result.HintCount > 0) ||
                    (scope.filter == 'success' && scope.result.SuccessCount > 0));
            });

            scope.sortOrder = ['-ErrorCount', '-WarningCount', '-HintCount'];

            var orderedItems = $filter('orderBy')(scope.result.Analysis.Results, scope.sortOrder);

            // A not so pretty way to divide the values in 2 columns
            scope.column1 = [];
            scope.column2 = [];

            angular.forEach(orderedItems, function (value, key) {
                if (key % 2 == 0) {
                    scope.column1.push(value);
                } else {
                    scope.column2.push(value);
                }
            });
        }
    }
});