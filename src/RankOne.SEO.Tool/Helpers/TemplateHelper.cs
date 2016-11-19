using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class TemplateHelper
    {
        private readonly UmbracoHelper _umbracoHelper;

        public TemplateHelper()
        {
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        }

        public TemplateHelper(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public string GetNodeHtml(IPublishedContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content", "Content parameter cannot be null");
            }
            if (content.Id == 0)
            {
                throw new MissingFieldException("The Id of content is not set");
            }
            if (content.TemplateId == 0)
            {
                throw new MissingFieldException("The templateId of content is not set");
            }

            var htmlObject = _umbracoHelper.RenderTemplate(content.Id);
            return htmlObject != null ? htmlObject.ToHtmlString() : null;
        }
    }
}
