using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class AnalyzerStructureApiController : UmbracoAuthorizedApiController
    {
        private readonly IEnumerable<ISummary> _summaries;

        public AnalyzerStructureApiController() : this(RankOneContext.Instance)
        { }

        public AnalyzerStructureApiController(RankOneContext rankOneContext) : this(rankOneContext.Summaries.Value)
        { }

        public AnalyzerStructureApiController(IEnumerable<ISummary> summaries)
        {
            _summaries = summaries;
        }

        public IEnumerable<AnalyzerStructure> GetStructure()
        {
            var structure = new List<AnalyzerStructure>();
            foreach (var summary in _summaries)
            {
                structure.Add(new AnalyzerStructure
                {
                    Name = summary.Alias,
                    Analyzers = summary.Analyzers.Select(x => x.Alias)
                });
            }
            return structure;
        }
    }
}