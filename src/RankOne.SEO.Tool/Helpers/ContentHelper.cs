using System.Net;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class ContentHelper
    {
        private readonly UmbracoHelper _umbracoHelper;

        public ContentHelper()
        {
            _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        }

        public ContentHelper(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public string GetNodeHtml(IPublishedContent content)
        {
            var htmlString = "";
            if (content != null)
            {
                if (content.TemplateId > 0)
                {
                    var htmlObject = _umbracoHelper.RenderTemplate(content.Id);
                    htmlString = htmlObject.ToHtmlString();
                }
                else if (!string.IsNullOrEmpty(content.Url))
                {
                    // Fallback for when a template is not set (yes, I'm looking at you Merchello ;))
                    htmlString = new WebClient().DownloadString(content.UrlWithDomain());
                }
            }
            return htmlString;
        }
    }
}
