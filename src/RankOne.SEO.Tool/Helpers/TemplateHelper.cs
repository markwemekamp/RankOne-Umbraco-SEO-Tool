using RankOne.Interfaces;
using RankOne.Models;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class TemplateHelper : ITemplateHelper
    {
        private readonly IUmbracoComponentRenderer _umbracoComponentRenderer;

        public TemplateHelper() : this(RankOneContext.Instance)
        { }

        public TemplateHelper(IRankOneContext rankOneContext) : this(rankOneContext.UmbracoComponentRenderery.Value)
        { }

        public TemplateHelper(IUmbracoComponentRenderer umbracoComponentRenderer)
        {
            if (umbracoComponentRenderer == null) throw new ArgumentNullException(nameof(umbracoComponentRenderer));

            _umbracoComponentRenderer = umbracoComponentRenderer;
        }

        public string GetNodeHtml(IPublishedContent content)
        {
            if (content == null)  throw new ArgumentNullException("content", "Content parameter cannot be null");
            if (content.Id == 0)  throw new MissingFieldException("The Id of content is not set");
            if (content.TemplateId == 0) throw new MissingFieldException("The templateId of content is not set");

            var htmlObject = _umbracoComponentRenderer.RenderTemplate(content.Id);
            return htmlObject != null ? htmlObject.ToHtmlString() : null;
        }
    }
}