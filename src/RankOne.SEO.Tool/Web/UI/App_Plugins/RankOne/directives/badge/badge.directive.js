angular.module('umbraco').directive('badge', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/badge/badge.directive.html',
        scope: {
            result: '='
        },
        link: function (scope) {
            if (scope.result.ErrorCount > 0) {
                scope.cssClass = 'error-badge';
                scope.number = scope.result.ErrorCount;
            } else if (scope.result.WarningCount > 0) {
                scope.cssClass = 'warning-badge';
                scope.number = scope.result.WarningCount;
            } else if (scope.result.HintCount > 0) {
                scope.cssClass = 'hint-badge';
                scope.number = scope.result.HintCount;
            }
        }
    }
});