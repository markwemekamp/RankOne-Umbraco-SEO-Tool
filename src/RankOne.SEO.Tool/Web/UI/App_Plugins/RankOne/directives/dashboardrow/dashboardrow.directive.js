angular.module('umbraco').directive('dashboardrow', function ($compile, dialogService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/dashboardrow/dashboardrow.directive.html',
        scope: {
            row: '=',
            level: '='
        },
        link: function (scope, element) {
            if (scope.row.PageScore) {
                if (scope.row.PageScore.OverallScore < 33) {
                    scope.cssClass = 'error-background';
                } else if (scope.row.PageScore.OverallScore < 66) {
                    scope.cssClass = 'warning-background';
                }
            }

            if (angular.isArray(scope.row.Children) && scope.row.HasChildrenWithTemplate) {
                var nextrow = angular.element("<dashboardrow row='childRow' level='" + (scope.level + 1) + "' ng-repeat='childRow in row.Children'/>");
                nextrow.insertAfter(element);
                $compile(nextrow)(scope);
            }

            scope.viewDetails = function(row) {
                dialogService.open({
                    template: "/App_Plugins/RankOne/dialogs/pageinformation/pageinformation.html",
                    show: true,
                    dialogData: {
                        row: row
                    }
                });
            };
        }
    }
});