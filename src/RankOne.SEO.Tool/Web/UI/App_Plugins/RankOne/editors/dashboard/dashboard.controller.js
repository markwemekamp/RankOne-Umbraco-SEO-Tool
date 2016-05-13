(function () {

    // Controller
    function rankOne($scope, $http, editorState, scoreService, resultService, urlService, localizationService, dialogService, notificationsService) {

        $scope.analyzeResults = null;
        $scope.filter = null;

        $scope.load = function () {
            $scope.loading = true;
            if (!editorState.current.template) {
                $scope.error = localizationService.localize("error_no_template");
                $scope.loading = false;
            } else {
                var relativeUrl = editorState.current.urls[0];

                if (relativeUrl == "This item is not published") {
                    $scope.error = localizationService.localize("error_not_published");
                    $scope.loading = false;
                } else {

                    var url = urlService.GetUrl(relativeUrl);

                    $http({
                        method: 'GET',
                        url: '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?url=' + url + '&focusKeyword=' + $scope.model.value.focusKeyword
                    }).then(function successCallback(response) {

                        if (response.data && response.data.Status == 200) {
                            $scope.analyzeResults = response.data;

                            var results = resultService.SetMetadata($scope.analyzeResults);

                            $scope.overallScore = scoreService.getOverallScore(results);
                            $scope.errorCount = scoreService.getTotalErrorCount(results);
                            $scope.warningCount = scoreService.getTotalWarningCount(results);
                            $scope.hintCount = scoreService.getTotalHintCount(results);
                            $scope.successCount = scoreService.getTotalSuccessCount(results);
                        } else {
                            $scope.error = localizationService.localize("error_page_error", [response.data.Status]);
                        }

                        $scope.loading = false;

                    }, function errorCallback(response) {
                        $scope.error = response.data.Message;
                        $scope.loading = false;
                    });
                }
            }
        }

        $scope.openSettings = function () {
            dialogService.open({
                template: "/App_Plugins/RankOne/dialogs/settings/settings.html",
                show: true,
                callback: function (data) {
                    $scope.model.value = data;
                    notificationsService.warning("Changes pending", "Changes will be saved when the node is saved");
                },
                dialogData: {
                    configuration: $scope.model.value
                }
            });
        };

        $scope.setFilter = function (filter) {
            $scope.filter = filter;
        };

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneDashboard', rankOne);

})();