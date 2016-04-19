angular.module('umbraco').directive('checkboxlist', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/checkboxlist/checkboxlist.directive.html',
        scope: {
            prevalues: '=',
            model: '='
        },
        link: function(scope) {
            if (!scope.model) {
                scope.model = [];
            }

            if (scope.model.length == 0) {
                angular.forEach(scope.prevalues, function(prevalue) {
                    scope.model.push(
                        {
                            name: prevalue,
                            checked: true
                        }
                    );
                });
            }
        }
    }
});