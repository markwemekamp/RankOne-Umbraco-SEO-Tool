using RankOne.Interfaces;
using RankOne.Models;
using System;
using Umbraco.Core.Models;

namespace RankOne.Services
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly IFocusKeywordHelper _focusKeywordHelper;
        private readonly IPageAnalysisService _pageAnalysisService;
        private readonly IAnalysisCacheRepository _analysisCacheService;

        public AnalyzeService() : this(RankOneContext.Instance)
        { }

        public AnalyzeService(RankOneContext rankOneContext) : this(rankOneContext.FocusKeywordHelper.Value, rankOneContext.PageAnalysisService.Value, 
            rankOneContext.AnalysisCacheRepository.Value)
        { }

        public AnalyzeService(IFocusKeywordHelper focusKeywordHelper, IPageAnalysisService pageAnalysisService, IAnalysisCacheRepository analysisCacheRepository)
        {
            _focusKeywordHelper = focusKeywordHelper;
            _pageAnalysisService = pageAnalysisService;
            _analysisCacheService = analysisCacheRepository;
        }

        public PageAnalysis CreateAnalysis(IPublishedContent node, string focusKeyword = null)
        {
            if (node.TemplateId == 0)
            {
                throw new MissingFieldException("TemplateId is not set");
            }

            if (string.IsNullOrEmpty(focusKeyword))
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