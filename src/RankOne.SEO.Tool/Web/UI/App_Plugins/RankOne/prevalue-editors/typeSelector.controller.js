(function () {

    // Controller
    function typeSelectorController($scope) {

        $scope.prevalues = ["Error", "Warning", "Hint", "Success"];
    };

    // Register the controller
    angular.module("umbraco").controller('typeSelectorController', typeSelectorController);

})();