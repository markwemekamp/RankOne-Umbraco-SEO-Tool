(function () {

    // Controller
    function typeSelectorController($scope) {

        $scope.prevalues = ["Error", "Warning", "Hint", "Success"];

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        if ($scope.model.value.length == 0) {
            angular.forEach($scope.prevalues, function (prevalue) {
                $scope.model.value.push(
                    {
                        name: prevalue,
                        checked: true
                    }
                );
            }); 
        }
    };

    // Register the controller
    angular.module("umbraco").controller('typeSelectorController', typeSelectorController);

})();