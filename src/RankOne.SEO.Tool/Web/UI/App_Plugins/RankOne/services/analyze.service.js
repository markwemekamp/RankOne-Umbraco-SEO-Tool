angular.module('umbraco')
    .service('analyzeService',
    function (webresultService, umbRequestHelper) {
        this.AnalyzeNode = function (nodeId) {
            var url = '/umbraco/backoffice/rankone/AnalysisApi/AnalyzeNode?id=' + nodeId;
            return webresultService.GetResult(url);
        },
            this.AnalyzeNodeForEditor = function (editor) {
                var url = umbRequestHelper.getApiUrl("AnalysisApi", "AnalyzeNode", "id={id}");
                return webresultService.GetResultFromEditorState(editorState, url)
            },
            this.AnalyzeNodeForEditorByKeyword = function (editor, focuskeyword) {
                var url = umbRequestHelper.getApiUrl("AnalysisApi", "AnalyzeNode", "id={id}&focusKeyword=" + focuskeyword);
                return webresultService.GetResultFromEditorState(editorState, url)
            },
            this.GetPageInformationForEditor = function (editor) {
                var url = umbRequestHelper.getApiUrl("PageApi", "GetPageInformation", "id={id}");
                return webresultService.GetResultFromEditorState(editorState, url)
            },
            this.GetStructure = function (editor) {
                var url = umbRequestHelper.getApiUrl("AnalyzerStructureApi", "GetStructure");
                return webresultService.GetResult(url);
            };

    });