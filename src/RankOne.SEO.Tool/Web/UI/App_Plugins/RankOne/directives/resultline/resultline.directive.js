angular.module('umbraco').directive('resultline', function (localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/resultline.directive.html',
        scope: {
            resultline: '='
        },
        link: function (scope) {
            scope.text = localizationService.localize(scope.resultline.Code, scope.resultline.Tokens);
        }
    }
});