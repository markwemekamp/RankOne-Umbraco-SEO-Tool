(function () {

    // Controller
    function rankOne($scope, $http, editorState, urlService, localizationService) {

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
                        url: '/umbraco/backoffice/api/RankOneApi/GetPageInformation?url=' + url
                    }).then(function successCallback(response) {

                        if (response.data && response.data.Status == 200) {
                            $scope.result = response.data;
                            $scope.loading = false;
                        } else {
                            $scope.error = localizationService.localize("error_page_error");
                        }

                        $scope.loading = false;
                    }, function errorCallback(response) {
                        $scope.error = response.data.Message;
                        $scope.loading = false;
                    });
                }
            }
        }

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneResultPreview', rankOne);

})();