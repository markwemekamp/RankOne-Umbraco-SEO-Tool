using System;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using Umbraco.Core.Models;

namespace RankOne.Services
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly FocusKeywordHelper _focusKeywordHelper;
        private readonly PageAnalysisService _pageAnalysisService;

        public AnalyzeService()
        {
            _focusKeywordHelper = new FocusKeywordHelper();
            _pageAnalysisService = new PageAnalysisService();
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

            return _pageAnalysisService.CreatePageAnalysis(node, focusKeyword);
        }
    }
}
