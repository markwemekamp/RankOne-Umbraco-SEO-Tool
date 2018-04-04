angular.module('umbraco')
    .service('rankOneRequestHelper',
    function (umbRequestHelper) {
        this.GetApiUrl = function (apiName, actionName, queryStrings) {
            return Umbraco.Sys.ServerVariables["RankOne"][apiName] + actionName + (!queryStrings ? "" : "?" + (angular.isString(queryStrings) ? queryStrings : umbRequestHelper.dictionaryToQueryString(queryStrings)));
        }
    });