using RankOne.Helpers;
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
        private readonly IConfigurationHelper _configurationHelper;

        public AnalyzerStructureApiController() : this(new ConfigurationHelper())
        { }

        public AnalyzerStructureApiController(ConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public IEnumerable<AnalyzerStructure> GetStructure()
        {
            var summaries = _configurationHelper.GetSummaries();

            var structure = new List<AnalyzerStructure>();
            foreach (var summary in summaries)
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
