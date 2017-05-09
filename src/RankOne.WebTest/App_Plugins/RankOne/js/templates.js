angular.module('umbraco').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('/App_Plugins/RankOne/directives/accordion/accordion.directive.html',
    "<div class=\"clearfix expander\"><label ng-click=\"expanded = !expanded\" ng-class=\"{'expanded':expanded}\"><localize key={{result.Alias}}_title></localize><badge result=result></badge></label><section ng-show=expanded><information information=information ng-repeat=\"information in result.Analysis.Information\"></information><div class=columns><div class=column><result result=analyzeResult ng-repeat=\"analyzeResult in column1\"></result></div><div class=column><result result=analyzeResult ng-repeat=\"analyzeResult in column2\"></result></div></div></section></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/badge/badge.directive.html',
    "<span class=\"badge {{cssClass}}\" ng-show=\"number > 0\">{{number}}</span>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/checkboxlist/checkboxlist.directive.html',
    "<ul class=unstyled><li ng-repeat=\"prevalue in model\"><label class=checkbox><input type=checkbox ng-model=prevalue.checked value={{prevalue.name}}> {{prevalue.name}}</label></li></ul>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/dashboardrow/dashboardrow.directive.html',
    "<div class=\"dashboard-row {{cssClass}}\" ng-click=\"!row.PageScore || viewDetails(row)\" ng-show=\"row.NodeInformation.TemplateId > 0 || row.HasChildrenWithTemplate\"><div class=\"segment row-title\"><label class=indentation-{{level}}><a href=#/content/content/edit/{{row.NodeInformation.Id}}>{{row.NodeInformation.Name}}</a></label></div><div class=\"segment score\"><span ng-if=row.PageScore>{{row.PageScore.OverallScore}}%</span></div><div class=segment><span ng-if=!row.PageScore title=\"There is no information available, try clicking the Refresh button.\">No information available</span> <span ng-if=row.PageScore.ErrorCount class=\"badge error-badge\" title=\"{{row.PageScore.ErrorCount}} errors\">{{row.PageScore.ErrorCount}} </span><span ng-if=row.PageScore.WarningCount class=\"badge warning-badge\" title=\"{{row.PageScore.WarningCount}} warnings\">{{row.PageScore.WarningCount}} </span><span ng-if=row.PageScore.HintCount class=\"badge hint-badge\" title=\"{{row.PageScore.HintCount}} hints\">{{row.PageScore.HintCount}}</span></div><div class=segment><span ng-if=\"row.FocusKeyword && row.FocusKeyword != 'undefined'\">{{row.FocusKeyword}}</span> <span ng-if=\"!row.FocusKeyword || row.FocusKeyword == 'undefined'\">No focuskeyword set</span></div></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/icon/icon.directive.html',
    "<div><div class=\"status {{colorClass}}\"><i class=icon-{{icon}}></i></div><span class=information-icon ng-click=openInformation() ng-if=hasGuidelines title={{moreInformation}}><i class=icon-info></i></span></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/information/information.directive.html',
    "<div class=information><div class=icon-container><i class=icon-info></i></div><span ng-bind-html=text></span></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/result/result.directive.html',
    "<div class=\"result clearfix\"><h3><localize key={{result.Alias}}_title></localize></h3><icon result=result></icon><div class=resultlines><ul><resultline resultline=resultLine result=result ng-repeat=\"resultLine in result.ResultRules\"></resultline></ul></div></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/resultline/resultline.directive.html',
    "<li class=\"resultline {{style}}\"><span ng-bind-html=text></span></li>"
  );


  $templateCache.put('/App_Plugins/RankOne/directives/summary/summary.directive.html',
    "<div><div ng-show=loading><localize key=summary_loading></localize></div><div ng-show=error>{{error}}</div><div ng-show=\"!loading && !error\"><div ng-repeat=\"analyzerResult in analyzeResults.AnalyzerResults | typeFilter:model.config.typeSelection | analyzerFilter:model.config.analyzerSelection\"><h4><localize key={{analyzerResult.Alias}}_title></localize></h4><div class=result-compact ng-repeat=\"result in analyzerResult.Analysis.Results | typeFilter:model.config.typeSelection | analyzerFilter:model.config.analyzerSelection\"><strong><localize key={{result.Alias}}_title></localize></strong><ul><resultline resultline=resultLine result=result ng-repeat=\"resultLine in result.ResultRules | typeFilter:model.config.typeSelection | orderBy:sortOrder\"></resultline></ul></div></div></div></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/prevalue-editors/analyzerSelector/analyzerSelector.html',
    "<div class=rank-one ng-controller=analyzerSelectorController><ul class=unstyled><li ng-repeat=\"analyzerSummary in model.value\"><label class=checkbox><input type=checkbox ng-model=analyzerSummary.checked value={{analyzerSummary}} class=checkbox-level1><localize key={{analyzerSummary.name}}_title></localize></label><ul class=\"unstyled level2\"><li ng-repeat=\"analyzer in analyzerSummary.analyzers\"><label class=checkbox><input type=checkbox ng-model=analyzer.checked value={{analyzer}} class=checkbox-level2><localize key={{analyzer.name}}_title></localize></label></li></ul></li></ul></div>"
  );


  $templateCache.put('/App_Plugins/RankOne/prevalue-editors/typeSelector/typeSelector.html',
    "<div class=rank-one ng-controller=typeSelectorController><checkboxlist types=types model=model.value></checkboxlist></div>"
  );

}]);
