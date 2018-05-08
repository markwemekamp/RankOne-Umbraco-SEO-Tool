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

        public AnalyzeService() : this(RankOneContext.Instance)
        { }

        public AnalyzeService(IRankOneContext rankOneContext) : this(rankOneContext.FocusKeywordHelper.Value, rankOneContext.PageAnalysisService.Value)
        { }

        public AnalyzeService(IFocusKeywordHelper focusKeywordHelper, IPageAnalysisService pageAnalysisService)
        {
            if (focusKeywordHelper == null) throw new ArgumentNullException(nameof(focusKeywordHelper));
            if (focusKeywordHelper == null) throw new ArgumentNullException(nameof(focusKeywordHelper));

            _focusKeywordHelper = focusKeywordHelper;
            _pageAnalysisService = pageAnalysisService;
        }

        public PageAnalysis CreateAnalysis(IPublishedContent node, string focusKeyword = null)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.TemplateId == 0) throw new MissingFieldException("TemplateId is not set");

            if (string.IsNullOrEmpty(focusKeyword))
            {
                focusKeyword = _focusKeywordHelper.GetFocusKeyword(node);
            }

            var analysis = _pageAnalysisService.CreatePageAnalysis(node, focusKeyword);

            SavePageAnalysis(node.Id, analysis);

            return analysis;
        }

        private void SavePageAnalysis(int pageId, PageAnalysis analysis)
        {
            _pageAnalysisService.Save(pageId, analysis);
        }
    }
}