angular.module('umbraco').directive('icon', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/icon/icon.directive.html',
        scope: {
            mode: '='
        },
        link: function (scope) {
            
            if (scope.mode == "succes") {
                scope.icon = "check info";
            }

            if (scope.mode == "error") {
                scope.icon = "delete error";
            }

            if (scope.mode == "warning") {
                scope.icon = "alert warning";
            }

            if (scope.mode == "hint") {
                scope.icon = "lightbulb-active color-blue";
            }
        }
    }
});