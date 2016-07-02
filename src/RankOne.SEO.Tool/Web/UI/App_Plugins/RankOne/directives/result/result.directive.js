angular.module('umbraco').directive('result', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/result/result.directive.html',
        scope: {
            result: '='
        }
    }
});