angular.module("umbraco").filter('typeFilter', function () {
    return function (items, types) {
        if (types != null) {
            var filteredItems = [];
            angular.forEach(items, function (item) {
                var added = false;
                angular.forEach(types, function(type) {
                    if (!added && type.checked) {
                        if (type.name == "Error" && (item.errorCount > 0 || item.Type == "error")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name == "Warning" && (item.warningCount > 0 || item.Type == "warning")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name == "Hint" && (item.hintCount > 0 || item.Type == "hint")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name == "Success" && (item.successCount > 0 || item.Type == "success")) {
                            filteredItems.push(item);
                            added = true;
                        }
                    }
                });
            });
            return filteredItems;
        }
        return items;
    };
});