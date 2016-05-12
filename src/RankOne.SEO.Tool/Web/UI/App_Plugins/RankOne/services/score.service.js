angular.module('umbraco')
    .service('scoreService',
        function () {

            function getScoreForResultInternal(result) {
                var score = 100;
                if (result) {

                    if (result.errorCount > 0) {
                        score = 0;
                    }
                    score -= result.warningCount * 50;
                    score -= result.hintCount * 25;

                    if (score < 0) {
                        score = 0;
                    }
                }
                return score;
            }


            this.getScoreForResult = function (result) {
                return getScoreForResultInternal(result);
            }
            this.getOverallScore = function (results) {
                var combinedScore = 0;
                angular.forEach(results, function (result) {
                    combinedScore += getScoreForResultInternal(result);
                });
                return Math.round(combinedScore / results.length);
            }

            this.getTotalErrorCount = function (results) {
                var combinedCount = 0;
                angular.forEach(results, function (result) {
                    combinedCount += result.errorCount;
                });
                return combinedCount;
            }

            this.getTotalWarningCount = function (results) {
                var combinedCount = 0;
                angular.forEach(results, function (result) {
                    combinedCount += result.warningCount;
                });
                return combinedCount;
            }

            this.getTotalHintCount = function (results) {
                var combinedCount = 0;
                angular.forEach(results, function (result) {
                    combinedCount += result.hintCount;
                });
                return combinedCount;
            }

            this.getTotalSuccessCount = function (results) {
                var combinedCount = 0;
                angular.forEach(results, function (result) {
                    combinedCount += result.successCount;
                });
                return combinedCount;
            }
        });