using System.Collections.Generic;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Repositories;
using Umbraco.Web;

namespace RankOne.Services
{
    public class DashboardDataService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IPageScoreNodeHelper _pageScoreNodeHelper;
        private readonly SchemaRepository<NodeReport> _schemaRepository;

        public DashboardDataService()
        {
            _pageScoreNodeHelper = new PageScoreNodeHelper();
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            _schemaRepository = new SchemaRepository<NodeReport>();
        }

        public void Initialize()
        {
            if (!_schemaRepository.DatabaseExists())
            {
                _schemaRepository.CreateTable();
            }
        }

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> the score from the database is used, else it will be calculated.</param>
        /// <returns></returns>
        public List<PageScoreNode> GetHierarchy(bool useCache = true)
        {
            if (_schemaRepository.DatabaseExists())
            {
                var nodeCollection = _umbracoHelper.TypedContentAtRoot();
                return _pageScoreNodeHelper.GetPageHierarchy(nodeCollection, useCache);
            }
            return null;
        }
    }
}
