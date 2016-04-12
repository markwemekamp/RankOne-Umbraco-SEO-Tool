angular.module('umbraco').directive('circle', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/circle/circle.directive.html',
        scope: {
            progress: '=',
            mode: '@mode'
        }
    }
});