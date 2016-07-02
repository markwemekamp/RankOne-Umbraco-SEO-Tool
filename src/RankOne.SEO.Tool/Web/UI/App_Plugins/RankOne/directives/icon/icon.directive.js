angular.module('umbraco').directive('icon', function (dialogService, localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/icon/icon.directive.html',
        scope: {
            result: '='
        }, link: function (scope) {
            scope.colorClass = 'info';
            scope.icon = 'check';
            if (scope.result.ErrorCount > 0) {
                scope.colorClass = 'error';
                scope.icon = 'delete';
            }
            else if (scope.result.WarningCount > 0) {
                scope.colorClass = 'warning';
                scope.icon = 'alert';
            }
            else if (scope.result.HintCount > 0) {
                scope.colorClass = 'pointer';
                scope.icon = 'lightbulb-active';
            }

            localizationService.localize(scope.result.Alias + "_guidelines").then(function (value) {
                scope.hasGuidelines = value && value.lastIndexOf('[', 0) === -1;
            });

            localizationService.localize('dashboard_more_information').then(function (value) {
                scope.moreInformation = value;
            });

            scope.openInformation = function () {
                dialogService.open({
                    template: "/App_Plugins/RankOne/dialogs/information/information.html",
                    show: true,
                    dialogData: {
                        result: scope.result
                    }
                });
            };
        }
    }
});