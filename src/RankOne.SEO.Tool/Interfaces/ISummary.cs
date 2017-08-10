using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface ISummary
    {
        string Name { get; set; }
        string Alias { get; set; }
        string FocusKeyword { get; set; }
        HtmlResult HtmlResult { get; set; }
        string Url { get; set; }
        IEnumerable<IAnalyzer> Analyzers { get; set; }
        Analysis GetAnalysis();
    }
}
