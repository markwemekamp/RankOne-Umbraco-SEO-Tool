angular.module('umbraco').directive('badge', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/badge/badge.directive.html',
        scope: {
            result: '='
        },
        link: function (scope) {
            if (scope.result.errorCount > 0) {
                scope.cssClass = 'error-badge';
                scope.number = scope.result.errorCount;
            }else if (scope.result.warningCount > 0) {
                scope.cssClass = 'warning-badge';
                scope.number = scope.result.warningCount;
            }else if (scope.result.hintCount > 0) {
                scope.cssClass = 'hint-badge';
                scope.number = scope.result.hintCount;
            }
        }
    }
});