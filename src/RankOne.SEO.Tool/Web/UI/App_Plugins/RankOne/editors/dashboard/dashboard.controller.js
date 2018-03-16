(function () {
    // Controller
    function rankOne($scope, webresultService, editorState, dialogService, notificationsService, localizationService, analyzeService) {
        $scope.analyzeResults = null;
        $scope.filter = null;

        $scope.load = function () {
            $scope.loading = true;

            if (!editorState.current.published) {
                $scope.error = localizationService.localize("error_not_published");
                $scope.loading = false;
            } else {
                analyzeService.AnalyzeNodeForEditorByKeyword(editorState.current, $scope.model.value.focusKeyword)
                    .then(function (response) {
                        $scope.analyzeResults = response;

                        $scope.overallScore = $scope.analyzeResults.Score.OverallScore;
                        $scope.errorCount = $scope.analyzeResults.Score.ErrorCount;
                        $scope.warningCount = $scope.analyzeResults.Score.WarningCount;
                        $scope.hintCount = $scope.analyzeResults.Score.HintCount;
                        $scope.successCount = $scope.analyzeResults.Score.SuccessCount;

                        $scope.loading = false;

                        $scope.color = '#4db53c';
                        if ($scope.overallScore < 33) {
                            $scope.color = '#dd2222';
                        } else if ($scope.overallScore < 66) {
                            $scope.color = '#dd9d22';
                        }

                        $("#score")
                            .circliful({
                                percent: $scope.overallScore,
                                fontColor: '#000000',
                                foregroundColor: $scope.color,
                                percentageTextSize: 30
                            });
                    },
                    function (message) {
                        $scope.error = "An error occured";
                        $scope.loading = false;
                    });
            }
        }

        $scope.openSettings = function () {
            dialogService.open({
                template: "/App_Plugins/RankOne/dialogs/settings/settings.html",
                show: true,
                callback: function (data) {
                    $scope.model.value = data;
                    localizationService.localize("warning_changes_pending_title")
                        .then(function (title) {
                            localizationService.localize("warning_changes_pending_text")
                                .then(function (text) {
                                    notificationsService
                                        .warning(title, text);
                                });
                        });
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