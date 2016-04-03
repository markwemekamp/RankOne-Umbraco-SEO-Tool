angular.module('umbraco').directive('circle', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/circle.directive.html',
        scope: {
            progress: '=',
            mode: '@mode'
        },
        link: function (scope, element, attrs, controller) {

        }
    }
});