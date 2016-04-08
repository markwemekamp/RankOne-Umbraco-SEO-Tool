angular.module('umbraco').directive('accordion', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/section.directive.html',
        transclude: true,
        scope: {
            result: '='
        },
        link: function (scope, element, attrs, controller) {
            scope.sortOrder = ['-errorCount', '-warningCount', '-hintCount'];
        }
    }
});