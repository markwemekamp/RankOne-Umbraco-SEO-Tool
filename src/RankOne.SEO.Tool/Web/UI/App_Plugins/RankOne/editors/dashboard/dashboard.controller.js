(function () {

    // Controller
    function rankOne($scope, $http, webresultService, editorState, scoreService, resultService, dialogService, notificationsService) {

        $scope.analyzeResults = null;
        $scope.filter = null;

        $scope.load = function () {
            $scope.loading = true;

            var url = '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?url={url}&focusKeyword=' + $scope.model.value.focusKeyword;
            webresultService.GetResult(editorState.current, url).then(function (response) {

                $scope.analyzeResults = response;

                var results = resultService.SetMetadata($scope.analyzeResults);

                $scope.overallScore = scoreService.getOverallScore(results);
                $scope.errorCount = scoreService.getTotalErrorCount(results);
                $scope.warningCount = scoreService.getTotalWarningCount(results);
                $scope.hintCount = scoreService.getTotalHintCount(results);
                $scope.successCount = scoreService.getTotalSuccessCount(results);

                $scope.loading = false;

                $scope.color = '#4db53c';
                if ($scope.overallScore < 33) {
                    $scope.color = '#dd2222';
                }
                else if ($scope.overallScore < 66) {
                    $scope.color = '#dd9d22';
                }

                $("#score").circliful({
                    percent: $scope.overallScore,
                    fontColor: '#000000',
                    foregroundColor: $scope.color,
                    percentageTextSize: 30
                });

            }, function (message) {
                $scope.error = message;
                $scope.loading = false;
            });
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