using RankOne.Analyzers;
using RankOne.Interfaces;

namespace RankOne.Tests.Mocks
{
    public class AnalyzerMock : BaseAnalyzer
    {
        public override void Analyse(IPageData pageData)
        {
            throw new System.NotImplementedException();
        }
    }
}
