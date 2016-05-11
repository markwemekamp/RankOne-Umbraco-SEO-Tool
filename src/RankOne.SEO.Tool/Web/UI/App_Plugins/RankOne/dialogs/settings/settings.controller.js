(function () {

    // Controller
    function rankOneSettings($scope) {
        $scope.loading = false;

        $scope.save = function () {
            $scope.submit($scope.dialogData.configuration);
        };
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSettings', rankOneSettings);

})();