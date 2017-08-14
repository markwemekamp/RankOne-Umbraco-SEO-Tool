(function () {
    // Controller
    function typeSelectorController($scope) {
        $scope.types = ["Error", "Warning", "Hint", "Success"];
    };

    // Register the controller
    angular.module("umbraco").controller('typeSelectorController', typeSelectorController);
})();