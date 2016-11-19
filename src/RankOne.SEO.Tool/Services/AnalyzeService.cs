using System;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Repositories;
using Umbraco.Core.Models;

namespace RankOne.Services
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly FocusKeywordHelper _focusKeywordHelper;
        private readonly PageAnalysisService _pageAnalysisService;
        private readonly AnalysisCacheRepository _analysisCacheService;

        public AnalyzeService()
        {
            _focusKeywordHelper = new FocusKeywordHelper();
            _pageAnalysisService = new PageAnalysisService();
            _analysisCacheService = new AnalysisCacheRepository();
        }

        public PageAnalysis CreateAnalysis(IPublishedContent node, string focusKeyword = null)
        {
            if (node.TemplateId == 0)
            {
                throw new MissingFieldException("TemplateId is not set");
            }

            if (!string.IsNullOrEmpty(focusKeyword))
            {
                focusKeyword = _focusKeywordHelper.GetFocusKeyword(node);
            }

            var analysis = _pageAnalysisService.CreatePageAnalysis(node, focusKeyword);

            CreateCachedAnalysisItem(node.Id, analysis);

            return analysis;
        }

        private void CreateCachedAnalysisItem(int pageId, PageAnalysis analysis)
        {
            _analysisCacheService.Save(pageId, analysis);
        }
    }
}
