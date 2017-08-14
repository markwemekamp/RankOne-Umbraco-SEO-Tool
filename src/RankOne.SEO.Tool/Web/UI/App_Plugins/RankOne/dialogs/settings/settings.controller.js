(function () {
    function rankOneSettings($scope) {
        $scope.loading = false;

        $scope.save = function () {
            $scope.submit($scope.dialogData.configuration);
        };
    };

    angular.module("umbraco").controller('rankOneSettings', rankOneSettings);
})();