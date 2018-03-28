using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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

        public AnalyzerStructureApiController(IRankOneContext rankOneContext) : this(rankOneContext.Summaries.Value)
        { }

        public AnalyzerStructureApiController(IEnumerable<ISummary> summaries)
        {
            if (summaries == null) throw new ArgumentNullException(nameof(summaries));

            _summaries = summaries;
        }

        public IHttpActionResult GetStructure()
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
            return Ok(structure);
        }
    }
}