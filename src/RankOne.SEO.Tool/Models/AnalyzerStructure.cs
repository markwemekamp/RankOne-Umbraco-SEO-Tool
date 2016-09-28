using System.Collections.Generic;

namespace RankOne.Models
{
    public class AnalyzerStructure
    {
        public string Name { get; set; }
        public IEnumerable<string> Analyzers { get; set; }
    }
}
