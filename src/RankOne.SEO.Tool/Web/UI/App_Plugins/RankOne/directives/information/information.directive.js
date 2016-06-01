angular.module('umbraco').directive('information', function (localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/information/information.directive.html',
        scope: {
            information: '='
        },
        link: function (scope) {
            scope.text = localizationService.localize(scope.information.Alias, scope.information.Tokens);
        }
    }
});