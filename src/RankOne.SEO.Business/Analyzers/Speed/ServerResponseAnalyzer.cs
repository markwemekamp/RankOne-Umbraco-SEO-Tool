using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Speed
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
            var serverResponseAnalysisResultRule = new ResultRule { Code = "serverresponseanalyzer_responsetime", Type = serverResponseTime > 3 ? ResultType.Warning : ResultType.Success };
            serverResponseAnalysisResultRule.Tokens.Add(serverResponseTime.ToString());
            serverResponseAnalysis.ResultRules.Add(serverResponseAnalysisResultRule);

            return serverResponseAnalysis;
        }
    }
}
