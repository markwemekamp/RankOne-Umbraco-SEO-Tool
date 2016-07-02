using HtmlAgilityPack;
using RankOne.Models;

namespace RankOne.Analyzers.Speed
{
    public class ServerResponseAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var serverResponseTime = (long)additionalValues[0];

            var serverResponseAnalysis = new AnalyzeResult
            {
                Alias = "serverresponseanalyzer"
            };
            var serverResponseAnalysisResultRule = new ResultRule
            {
                Alias = "serverresponseanalyzer_responsetime",
                Type = serverResponseTime > 3000 ? ResultType.Warning : ResultType.Success
            };
            serverResponseAnalysisResultRule.Tokens.Add(serverResponseTime.ToString());
            serverResponseAnalysis.ResultRules.Add(serverResponseAnalysisResultRule);

            return serverResponseAnalysis;
        }
    }
}
