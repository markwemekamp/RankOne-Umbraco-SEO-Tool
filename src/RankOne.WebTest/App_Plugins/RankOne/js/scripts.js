(function () {

    // Controller
    function rankOneSiteDashboard($scope, dashboardService, localizationService) {

        $scope.analyzeResults = null;
        $scope.filter = null;
        $scope.initialized = false;

        $scope.load = function () {
            $scope.loading = true;
            dashboardService.GetPageHierarchy().then(loadData, showError);
        };

        $scope.refresh = function () {
            $scope.loading = true;
            $scope.pageHierarchy = null;
            dashboardService.UpdateAllPages().then(loadData, showError);
        };

        $scope.initialize = function () {
            $scope.loading = true;
            $scope.pageHierarchy = null;
            dashboardService.Initialize().then(loadData, showError);
        };

        function loadData(response) {
            if (response && response !== "null") {
                $scope.setData(response);
                $scope.initialized = true;
            }
            $scope.loading = false;
        }

        function showError(response) {
            $scope.error = response;
            $scope.loading = false;
        }

        $scope.setData = function (data) {

            // Remove the previously added rows from dashboardrow directive
            $('.dashboard-row').remove();
            $scope.pageHierarchy = data;

            var totalScore = 0;
            var items = 0;

            // sum the overallscore per node
            function addScore(item) {
                if (item.PageScore) {
                    totalScore += item.PageScore.OverallScore;
                    items++;
                }
                _.each(item.Children, addScore);
            }
            _.each(data, addScore);

            if (items > 0) {
                $scope.overallScore = Math.round(totalScore / items);
            } else {
                $scope.overallScore = 0;
            }

            $scope.color = '#4db53c';
            if ($scope.overallScore < 33) {
                $scope.color = '#dd2222';
            } else if ($scope.overallScore < 66) {
                $scope.color = '#dd9d22';
            }

            $scope.intro_text = localizationService.localize('sitedashboard_intro_text', [$scope.overallScore]);

            $("#score")
                .circliful({
                    percent: $scope.overallScore,
                    fontColor: '#000000',
                    foregroundColor: $scope.color,
                    percentageTextSize: 30
                });
        };

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSiteDashboard', rankOneSiteDashboard);

})();
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
(function () {

    function rankOneSettings($scope) {
        $scope.loading = false;

        $scope.save = function () {
            $scope.submit($scope.dialogData.configuration);
        };
    };

    angular.module("umbraco").controller('rankOneSettings', rankOneSettings);

})();
angular.module('umbraco').directive('accordion', function ($filter) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/accordion/accordion.directive.html',
        transclude: true,
        scope: {
            result: '=',
            filter: '='
        },
        link: function (scope) {

            scope.$watch('filter', function () {
                scope.expanded = scope.result && ((scope.filter === 'error' && scope.result.ErrorCount > 0) ||
                    (scope.filter === 'warning' && scope.result.WarningCount > 0) ||
                    (scope.filter === 'hint' && scope.result.HintCount > 0) ||
                    (scope.filter === 'success' && scope.result.SuccessCount > 0));
            });

            scope.sortOrder = ['-ErrorCount', '-WarningCount', '-HintCount'];

            var orderedItems = $filter('orderBy')(scope.result.Analysis.Results, scope.sortOrder);

            // A not so pretty way to divide the values in 2 columns
            scope.column1 = [];
            scope.column2 = [];

            angular.forEach(orderedItems, function (value, key) {
                if (key % 2 === 0) {
                    scope.column1.push(value);
                } else {
                    scope.column2.push(value);
                }
            });
        }
    }
});
angular.module('umbraco').directive('badge', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/badge/badge.directive.html',
        scope: {
            result: '='
        },
        link: function (scope) {
            if (scope.result.ErrorCount > 0) {
                scope.cssClass = 'error-badge';
                scope.number = scope.result.ErrorCount;
            }else if (scope.result.WarningCount > 0) {
                scope.cssClass = 'warning-badge';
                scope.number = scope.result.WarningCount;
            }else if (scope.result.HintCount > 0) {
                scope.cssClass = 'hint-badge';
                scope.number = scope.result.HintCount;
            }
        }
    }
});
angular.module('umbraco').directive('checkboxlist', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/checkboxlist/checkboxlist.directive.html',
        scope: {
            types: '=',
            model: '='
        },
        link: function(scope) {
            if (!scope.model) {
                scope.model = [];
            }

            if (scope.model.length === 0) {
                angular.forEach(scope.types, function(prevalue) {
                    scope.model.push(
                        {
                            name: prevalue,
                            checked: true
                        }
                    );
                });
            }
        }
    }
});
angular.module('umbraco').directive('dashboardrow', function ($compile, dialogService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/dashboardrow/dashboardrow.directive.html',
        scope: {
            row: '=',
            level: '='
        },
        link: function (scope, element) {

            if (scope.row.PageScore) {
                if (scope.row.PageScore.OverallScore < 33) {
                    scope.cssClass = 'error-background';
                } else if (scope.row.PageScore.OverallScore < 66) {
                    scope.cssClass = 'warning-background';
                }
            }

            if (angular.isArray(scope.row.Children) && scope.row.HasChildrenWithTemplate) {
                var nextrow = angular.element("<dashboardrow row='childRow' level='" + (scope.level + 1) + "' ng-repeat='childRow in row.Children'/>");
                nextrow.insertAfter(element);
                $compile(nextrow)(scope);
            }

            scope.viewDetails = function(row) {
                dialogService.open({
                    template: "/App_Plugins/RankOne/dialogs/pageinformation/pageinformation.html",
                    show: true,
                    dialogData: {
                        row: row
                    }
                });
            };
        }
    }
});
angular.module('umbraco').directive('icon', function (dialogService, localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/icon/icon.directive.html',
        scope: {
            result: '='
        }, link: function (scope) {
            scope.colorClass = 'info';
            scope.icon = 'check';
            if (scope.result.ErrorCount > 0) {
                scope.colorClass = 'error';
                scope.icon = 'delete';
            }
            else if (scope.result.WarningCount > 0) {
                scope.colorClass = 'warning';
                scope.icon = 'alert';
            }
            else if (scope.result.HintCount > 0) {
                scope.colorClass = 'pointer';
                scope.icon = 'lightbulb-active';
            }

            localizationService.localize(scope.result.Alias + "_guidelines").then(function (value) {
                scope.hasGuidelines = value && value.lastIndexOf('[', 0) === -1;
            });

            localizationService.localize('dashboard_more_information').then(function (value) {
                scope.moreInformation = value;
            });

            scope.openInformation = function () {
                dialogService.open({
                    template: "/App_Plugins/RankOne/dialogs/information/information.html",
                    show: true,
                    dialogData: {
                        result: scope.result
                    }
                });
            };
        }
    }
});
angular.module('umbraco').directive('information', function (localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/information/information.directive.html',
        scope: {
            information: '='
        },
        link: function (scope) {
            scope.text = localizationService.localize(scope.information.Alias, scope.information.Tokens);
        }
    }
});
angular.module('umbraco').directive('result', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/result/result.directive.html',
        scope: {
            result: '='
        }
    }
});
angular.module('umbraco').directive('resultline', function (localizationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/resultline/resultline.directive.html',
        scope: {
            resultline: '=',
            result: '='
        },
        link: function (scope) {
            scope.text = localizationService.localize(scope.result.Alias + '_' + scope.resultline.Alias, scope.resultline.Tokens);

            if (scope.resultline.Type === "success") {
                scope.style = "info";
            }

            if (scope.resultline.Type === "error") {
                scope.style = "error";
            }

            if (scope.resultline.Type === "warning") {
                scope.style = "warning";
            }

            if (scope.resultline.Type === "hint") {
                scope.style = "pointer";
            }
        }
    }
});
angular.module('umbraco').directive('summary', function (webresultService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/RankOne/directives/summary/summary.directive.html',
        scope: {
            nodeId: '=',
            model: '='
        },
        link: function (scope) {
            scope.sortOrder = ['-ErrorCount', '-WarningCount', '-HintCount'];

            scope.load = function () {
                scope.loading = true;

                var url = '/umbraco/backoffice/rankone/AnalysisApi/AnalyzeNode?id=' + scope.nodeId;
                webresultService.GetResult(url)
                    .then(function (response) {
                        scope.analyzeResults = response;
                        scope.loading = false;
                    },
                        function (message) {
                            scope.error = message;
                            scope.loading = false;
                        });
            };

            scope.load();
        }
    }
});
(function () {

    // Controller
    function rankOne($scope, webresultService, editorState, dialogService, notificationsService, localizationService) {

        $scope.analyzeResults = null;
        $scope.filter = null;

        $scope.load = function () {
            $scope.loading = true;

            if (!editorState.current.published) {
                $scope.error = localizationService.localize("error_not_published");
                $scope.loading = false;
            } else {
                var url = '/umbraco/backoffice/rankone/AnalysisApi/AnalyzeNode?id={id}&focusKeyword=' +
                    $scope.model.value.focusKeyword;
                webresultService.GetResultFromEditorState(editorState.current, url)
                    .then(function (response) {

                        $scope.analyzeResults = response;

                        $scope.overallScore = $scope.analyzeResults.Score.OverallScore;
                        $scope.errorCount = $scope.analyzeResults.Score.ErrorCount;
                        $scope.warningCount = $scope.analyzeResults.Score.WarningCount;
                        $scope.hintCount = $scope.analyzeResults.Score.HintCount;
                        $scope.successCount = $scope.analyzeResults.Score.SuccessCount;

                        $scope.loading = false;

                        $scope.color = '#4db53c';
                        if ($scope.overallScore < 33) {
                            $scope.color = '#dd2222';
                        } else if ($scope.overallScore < 66) {
                            $scope.color = '#dd9d22';
                        }

                        $("#score")
                            .circliful({
                                percent: $scope.overallScore,
                                fontColor: '#000000',
                                foregroundColor: $scope.color,
                                percentageTextSize: 30
                            });

                    },
                    function (message) {
                        $scope.error = message;
                        $scope.loading = false;
                    });
            }
        }

        $scope.openSettings = function () {
            dialogService.open({
                template: "/App_Plugins/RankOne/dialogs/settings/settings.html",
                show: true,
                callback: function (data) {
                    $scope.model.value = data;
                    localizationService.localize("warning_changes_pending_title")
                        .then(function (title) {
                            localizationService.localize("warning_changes_pending_text")
                                .then(function (text) {

                                    notificationsService
                                        .warning(title, text);
                                });
                        });
                },
                dialogData: {
                    configuration: $scope.model.value
                }
            });
        };

        $scope.setFilter = function (filter) {
            $scope.filter = filter;
        };

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneDashboard', rankOne);

})();
(function () {

    // Controller
    function rankOne($scope, editorState, webresultService, localizationService) {

        $scope.load = function () {
            $scope.loading = true;

            if (!editorState.current.published) {
                $scope.error = localizationService.localize("error_not_published");
                $scope.loading = false;
            } else {
                var url = '/umbraco/backoffice/rankone/PageApi/GetPageInformation?id={id}';
                webresultService.GetResultFromEditorState(editorState.current, url)
                    .then(function(response) {

                            $scope.result = response;
                            $scope.loading = false;

                        },
                        function(message) {
                            $scope.error = message;
                            $scope.loading = false;
                        });
            }
        }

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneResultPreview', rankOne);

})();
(function () {

    // Controller
    function rankOneSummary($scope, editorState, webresultService, localizationService) {
        $scope.load = function () {
            $scope.loading = true;

            if (!editorState.current.published) {
                $scope.error = localizationService.localize("error_not_published");
                $scope.loading = false;
            } else {
                var url = '/umbraco/backoffice/rankone/AnalysisApi/AnalyzeNode?id={id}';
                webresultService.GetResultFromEditorState(editorState.current, url)
                    .then(function (response) {
                        $scope.analyzeResults = response;
                        $scope.loading = false;
                    },
                        function (message) {
                            $scope.error = message;
                            $scope.loading = false;
                        });
            }
        };

        $scope.$on("formSubmitted", function () {
            $scope.load();
        });

        $scope.load();
        
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOneSummary);

})();
angular.module("umbraco").filter('analyzerFilter', function () {
    return function (items, analyzers) {
        if (analyzers != null) {
            var filteredItems = [];
            angular.forEach(items, function (item) {
                var added = false;
                angular.forEach(analyzers, function (analyzer) {
                    if (!added) {
                        if (analyzer.checked && item.Alias === analyzer.name) {
                            filteredItems.push(item);
                            added = true;
                        } else if (analyzer.analyzers.length > 0) {
                            angular.forEach(analyzer.analyzers, function(subAnalyzer) {
                                if (!added) {
                                    if (subAnalyzer.checked && item.Alias === subAnalyzer.name) {
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
angular.module("umbraco").filter('typeFilter', function () {
    return function (items, types) {
        if (types != null) {
            var filteredItems = [];
            angular.forEach(items, function (item) {
                var added = false;
                angular.forEach(types, function(type) {
                    if (!added && type.checked) {
                        if (type.name === "Error" && (item.ErrorCount > 0 || item.Type === "error")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name === "Warning" && (item.WarningCount > 0 || item.Type === "warning")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name === "Hint" && (item.HintCount > 0 || item.Type === "hint")) {
                            filteredItems.push(item);
                            added = true;
                        }
                        if (type.name === "Success" && (item.SuccessCount > 0 || item.Type === "success")) {
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
"use strict";

(function ($) {

    $.fn.circliful = function (options, callback) {

        var settings = $.extend({
            // These are the defaults.
            //startDegree: 0,
            foregroundColor: "#3498DB",
            backgroundColor: "#ccc",
            pointColor: "none",
            fillColor: 'none',
            foregroundBorderWidth: 15,
            backgroundBorderWidth: 15,
            pointSize: 28.5,
            fontColor: '#aaa',
            percent: 75,
            animation: 1,
            animationStep: 5,
            icon: 'none',
            iconSize: '30',
            iconColor: '#ccc',
            iconPosition: 'top',
            target: 0,
            start: 0,
            showPercent: 1,
            percentageTextSize: 22,
            textAdditionalCss: '',
            targetPercent: 0,
            targetTextSize: 17,
            targetColor: '#2980B9',
            text: null,
            textStyle: null,
            textColor: '#666',
            multiPercentage: 0,
            percentages: null
        }, options);

        return this.each(function () {
            var circleContainer = $(this);
            var percent = settings.percent;
            var iconY = 83;
            var iconX = 100;
            var textY = 110;
            var textX = 100;
            var additionalCss;
            var elements;
            var icon;
            var backgroundBorderWidth = settings.backgroundBorderWidth;

            if (settings.iconPosition == 'bottom') {
                iconY = 124;
                textY = 95;
            } else if (settings.iconPosition == 'left') {
                iconX = 80;
                iconY = 110;
                textX = 117;
            } else if (settings.iconPosition == 'middle') {
                if (settings.multiPercentage == 1) {
                    if (typeof settings.percentages == "object") {
                        backgroundBorderWidth = 30;
                    } else {
                        iconY = 110;
                        elements = '<g stroke="' + (settings.backgroundColor != 'none' ? settings.backgroundColor : '#ccc') + '" ><line x1="133" y1="50" x2="140" y2="40" stroke-width="2"  /></g>';
                        elements += '<g stroke="' + (settings.backgroundColor != 'none' ? settings.backgroundColor : '#ccc') + '" ><line x1="140" y1="40" x2="200" y2="40" stroke-width="2"  /></g>';
                        textX = 228;
                        textY = 47;
                    }
                } else {
                    iconY = 110;
                    elements = '<g stroke="' + (settings.backgroundColor != 'none' ? settings.backgroundColor : '#ccc') + '" ><line x1="133" y1="50" x2="140" y2="40" stroke-width="2"  /></g>';
                    elements += '<g stroke="' + (settings.backgroundColor != 'none' ? settings.backgroundColor : '#ccc') + '" ><line x1="140" y1="40" x2="200" y2="40" stroke-width="2"  /></g>';
                    textX = 175;
                    textY = 35;
                }
            } else if (settings.iconPosition == 'right') {
                iconX = 120;
                iconY = 110;
                textX = 80;
            }

            if (settings.targetPercent > 0) {
                textY = 95;
                elements = '<g stroke="' + (settings.backgroundColor != 'none' ? settings.backgroundColor : '#ccc') + '" ><line x1="75" y1="101" x2="125" y2="101" stroke-width="1"  /></g>';
                elements += '<text text-anchor="middle" x="' + textX + '" y="120" style="font-size: ' + settings.targetTextSize + 'px;" fill="' + settings.targetColor + '">' + settings.targetPercent + '%</text>';
                elements += '<circle cx="100" cy="100" r="69" fill="none" stroke="' + settings.backgroundColor + '" stroke-width="3" stroke-dasharray="450" transform="rotate(-90,100,100)" />';
                elements += '<circle cx="100" cy="100" r="69" fill="none" stroke="' + settings.targetColor + '" stroke-width="3" stroke-dasharray="' + (360 / 100 * settings.targetPercent) + ', 20000" transform="rotate(-90,100,100)" />';

            }

            if (settings.text != null && settings.multiPercentage == 0) {
                elements += '<text text-anchor="middle" x="100" y="125" style="' + settings.textStyle + '" fill="' + settings.textColor + '">' + settings.text + '</text>';
            } else if (settings.text != null && settings.multiPercentage == 1) {
                elements += '<text text-anchor="middle" x="228" y="65" style="' + settings.textStyle + '" fill="' + settings.textColor + '">' + settings.text + '</text>';
            }

            if (settings.icon != 'none') {
                icon = '<text text-anchor="middle" x="' + iconX + '" y="' + iconY + '" class="icon" style="font-size: ' + settings.iconSize + 'px" fill="' + settings.iconColor + '">&#x' + settings.icon + '</text>';
            }

            circleContainer.html('');

            circleContainer
                .addClass('svg-container')
                .append(
                    $('<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 194 186" class="circliful">' +
                        elements +
                        '<circle cx="100" cy="100" r="57" class="border" fill="' + settings.fillColor + '" stroke="' + settings.backgroundColor + '" stroke-width="' + backgroundBorderWidth + '" stroke-dasharray="360" transform="rotate(-90,100,100)" />' +
                        '<circle class="circle" cx="100" cy="100" r="57" class="border" fill="none" stroke="' + settings.foregroundColor + '" stroke-width="' + settings.foregroundBorderWidth + '" stroke-dasharray="0,20000" transform="rotate(-90,100,100)" />' +
                        '<circle cx="100" cy="100" r="' + settings.pointSize + '" fill="' + settings.pointColor + '" />' +
                        icon +
                        '<text class="timer" text-anchor="middle" x="' + textX + '" y="' + textY + '" style="font-size: ' + settings.percentageTextSize + 'px; ' + additionalCss + ';' + settings.textAdditionalCss + '" fill="' + settings.fontColor + '">0%</text>')
                );

            var circle = circleContainer.find('.circle');
            var myTimer = circleContainer.find('.timer');
            var interval = 30;
            var angle = 0;
            var angleIncrement = settings.animationStep;
            var last = 0;
            var summary = 0;
            var oneStep = 0;

            if (settings.start > 0 && settings.target > 0) {
                percent = settings.start / (settings.target / 100);
                oneStep = settings.target / 100;
            }

            if (settings.animation == 1) {
                var timer = window.setInterval(function () {
                    if ((angle) >= (360 / 100 * percent)) {
                        window.clearInterval(timer);
                        last = 1;
                    } else {
                        angle += angleIncrement;
                        summary += oneStep;
                    }

                    if (angle / 3.6 >= percent && last == 1) {
                        angle = 3.6 * percent;
                    }

                    if (summary > settings.target && last == 1) {
                        summary = settings.target;
                    }

                    circle
                        .attr("stroke-dasharray", angle + ", 20000");

                    if (settings.showPercent == 1) {
                        myTimer
                            .text(parseInt(angle / 360 * 100) + '%');
                    } else {
                        myTimer
                            .text(summary);
                    }

                }.bind(circle), interval);
            } else {
                circle
                    .attr("stroke-dasharray", (360 / 100 * percent) + ", 20000");

                if (settings.showPercent == 1) {
                    myTimer
                        .text(percent + '%');
                } else {
                    myTimer
                        .text(settings.target);
                }
            }
        });
    }

}(jQuery));
(function () {

    // Controller
    function analyzerSelectorController($scope, webresultService) {

        var url = '/umbraco/backoffice/rankone/AnalyzerStructureApi/GetStructure';
        webresultService.GetResult(url)
            .then(function (response) {

                $scope.analyzerSummaries = response;
                $scope.load();
                $scope.loading = false;

            },
                function (message) {
                    $scope.error = message;
                    $scope.loading = false;
                });

        $scope.load = function () {

            var tempObject = [];
            var firstTime = false;
            if (!$scope.model.value) {
                $scope.model.value = [];
                firstTime = true;
            }

            angular.forEach($scope.analyzerSummaries,
                function (analyzerSummary) {

                    var analyzerSummaryObject = _.findWhere($scope.model.value, { name: analyzerSummary.Name });

                    if (!analyzerSummaryObject) {
                        analyzerSummaryObject = {
                            name: analyzerSummary.Name,
                            checked: firstTime,
                            analyzers: []
                        };

                    }
                    angular.forEach(analyzerSummary.Analyzers,
                            function (analyzer) {

                                var analyzerObject = _.findWhere(analyzerSummaryObject.analyzers, { name: analyzer });

                                if (!analyzerObject) {
                                    analyzerObject = {
                                        name: analyzer,
                                        checked: firstTime
                                    }
                                    analyzerSummaryObject.analyzers.push(analyzerObject);
                                }
                            });

                    tempObject.push(analyzerSummaryObject);
                });

            $scope.model.value = tempObject;
        }
    };

    // Register the controller
    angular.module("umbraco").controller('analyzerSelectorController', analyzerSelectorController);
})();

(function () {

    // Controller
    function typeSelectorController($scope) {

        $scope.types = ["Error", "Warning", "Hint", "Success"];
    };

    // Register the controller
    angular.module("umbraco").controller('typeSelectorController', typeSelectorController);

})();
angular.module('umbraco')
    .service('dashboardService',
        function (webresultService) {
            this.GetPageHierarchy = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/GetPageHierarchy';
                return webresultService.GetResult(url);
            },
            this.UpdateAllPages = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/UpdateAllPages';
                return webresultService.GetResult(url);
            },
            this.Initialize = function () {
                var url = '/umbraco/backoffice/rankone/DashboardApi/Initialize';
                return webresultService.GetResult(url);
            };
        });
angular.module('umbraco')
    .service('webresultService',
        function ($q, $http, localizationService) {
            this.GetResult = function (url) {
                var deferred = $q.defer();
                $http({ method: 'GET', url: url })
                    .then(function (response) {
                        if (response.data && response.status === 200) {
                            deferred.resolve(response.data);
                        } else {
                            deferred.reject(localizationService
                                .localize("error_page_error", [response.status]));
                        }
                    },
                    function (response) {
                        deferred.reject(response.data);
                    });

                return deferred.promise;
            },
            this.GetResultFromEditorState = function (editorState, url) {
                var deferred = $q.defer();

                url = url.replace("{id}", editorState.id);

                this.GetResult(url)
                    .then(function (response) {
                        deferred.resolve(response);
                    },
                    function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;

            };
    });