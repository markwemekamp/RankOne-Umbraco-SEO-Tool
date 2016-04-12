angular.module('umbraco')
    .service('resultService',
        function () {

            function countRulesForType(result, type) {
                return _.where(result.ResultRules, { Type: type }).length;
            }

            function SetMetadataForResult(result) {
                result.errorCount = countRulesForType(result, 'error');
                result.warningCount = countRulesForType(result, 'warning');
                result.hintCount = countRulesForType(result, 'hint');
                result.successCount = countRulesForType(result, 'success');
            }

            this.SetMetadata = function (analyzeResult) {
                var results = [];
                angular.forEach(analyzeResult.AnalyzerResults, function (analyzerResult) {
                    analyzerResult.errorCount = 0;
                    analyzerResult.warningCount = 0;
                    analyzerResult.hintCount = 0;
                    analyzerResult.successCount = 0;

                    angular.forEach(analyzerResult.Analysis.Results, function (result) {
                        SetMetadataForResult(result);
                        analyzerResult.errorCount += result.errorCount;
                        analyzerResult.warningCount += result.warningCount;
                        analyzerResult.hintCount += result.hintCount;
                        analyzerResult.successCount += result.successCount;
                        results.push(result);
                    });
                });
                return results;
            };
        });