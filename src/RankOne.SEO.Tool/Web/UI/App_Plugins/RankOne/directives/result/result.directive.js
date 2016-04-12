angular.module('umbraco').directive('result', function (scoreService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/result/result.directive.html',
        scope: {
            result: '='
        },
        link: function (scope) {
            scope.score = scoreService.getScoreForResult(scope.result);
        }
    }
});