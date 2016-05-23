angular.module('umbraco').directive('icon', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/icon/icon.directive.html',
        scope: {
            result: '='
        }, link: function (scope) {
            scope.colorClass = 'info';
            scope.icon = 'check';
            if (scope.result.errorCount > 0) {
                scope.colorClass = 'error';
                scope.icon = 'delete';
            }
            else if (scope.result.warningCount > 0) {
                scope.colorClass = 'warning';
                scope.icon = 'alert';
            }
            else if (scope.result.hintCount > 0) {
                scope.colorClass = 'pointer';
                scope.icon = 'lightbulb-active';
            }
        }
    }
});