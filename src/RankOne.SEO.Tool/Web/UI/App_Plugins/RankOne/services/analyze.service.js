angular.module('umbraco')
    .service('analyzeService',
    function (webresultService, rankOneRequestHelper) {
        this.AnalyzeNode = function (nodeId) {
            var url = rankOneRequestHelper.GetApiUrl("AnalysisApi", "AnalyzeNode", "id={nodeId}");
            return webresultService.GetResult(url);
        }
        this.AnalyzeNodeForEditor = function (editor) {
            var url = rankOneRequestHelper.GetApiUrl("AnalysisApi", "AnalyzeNode", "id={id}");
            return webresultService.GetResultFromEditorState(editorState, url)
        }
        this.AnalyzeNodeForEditorByKeyword = function (editorState, focuskeyword) {
            var url = rankOneRequestHelper.GetApiUrl("AnalysisApi", "AnalyzeNode", "id={id}&focusKeyword=" + focuskeyword);
            return webresultService.GetResultFromEditorState(editorState, url);
        }
        this.GetPageInformationForEditor = function (editorState) {
            var url = rankOneRequestHelper.GetApiUrl("PageApi", "GetPageInformation", "id={id}");
            return webresultService.GetResultFromEditorState(editorState, url);
        }
        this.GetStructure = function () {
            var url = rankOneRequestHelper.GetApiUrl("AnalyzerStructureApi", "GetStructure");
            return webresultService.GetResult(url);
        }
    });