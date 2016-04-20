angular.module("umbraco").filter('analyzerFilter', function () {
    return function (items, analyzers) {
        if (analyzers != null) {
            var filteredItems = [];
            angular.forEach(items, function (item) {
                var added = false;
                angular.forEach(analyzers, function (analyzer) {
                    if (!added) {
                        if (analyzer.checked && item.Alias == analyzer.name) {
                            filteredItems.push(item);
                            added = true;
                        } else if (analyzer.analyzers.length > 0) {
                            angular.forEach(analyzer.analyzers, function(subAnalyzer) {
                                if (!added) {
                                    if (subAnalyzer.checked && item.Alias == subAnalyzer.name) {
                                        filteredItems.push(item);
                                        added = true;
                                    }
                                }
                            });
                        }
                    }
                });
            });
            return filteredItems;
        }
        return items;
    };
});