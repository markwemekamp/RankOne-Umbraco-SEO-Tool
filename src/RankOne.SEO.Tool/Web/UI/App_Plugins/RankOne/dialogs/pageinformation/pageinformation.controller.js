(function () {
    function rankOnePageInformation($scope) {
        var model = {};
        model.config = {};
        model.config.typeSelection = [{ name: 'Error', checked: true }, { name: 'Warning', checked: true },
            { name: 'Hint', checked: true }];
        $scope.model = model;
    }

    angular.module("umbraco").controller('rankOnePageInformation', rankOnePageInformation);
})();