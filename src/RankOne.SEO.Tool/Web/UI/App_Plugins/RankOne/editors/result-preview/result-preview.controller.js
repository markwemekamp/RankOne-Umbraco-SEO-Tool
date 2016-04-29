(function () {

    // Controller
    function rankOne($scope, $http, editorState, urlService, localizationService) {
 um
        if (!editorState.current.template) {
            $scope.error = localizationService.localize("error_no_template");
        } else {
            var relativeUrl = editorState.current.urls[0];

            if (relativeUrl == "This item is not published") {
                $scope.error = localizationService.localize("error_not_published");
            } else {
                $scope.loading = true;
                var url = urlService.GetUrl(relativeUrl);

                $http({
                    method: 'GET',
                    url: '/umbraco/backoffice/api/RankOneApi/GetPageInformation?url=' + url
                }).then(function successCallback(response) {
                    
                    $scope.result = response.data;
                    $scope.loading = false;
                }, function errorCallback(response) {
                    console.log(response);
                });
            }
        }
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneResultPreview', rankOne);

})();