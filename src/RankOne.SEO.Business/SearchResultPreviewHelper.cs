using System.Linq;
using System.Xml.Linq;
using RankOne.Business;
using SEO.Umbraco.Extensions.Models;

namespace SEO.Umbraco.Extensions
{
    public class SearchResultPreviewHelper
    {
        private HtmlHelper _htmlHelper;

        public SearchResultPreviewHelper()
        {
            _htmlHelper = new HtmlHelper();
        }

        public SearchResultPreview GetSearchResultPreview(string url, XDocument document)
        {
            var searchResultPreview = new SearchResultPreview {Url = url};


            var titleTags = _htmlHelper.GetElements(document, "title");
            if (titleTags.Any())
            {
                searchResultPreview.Title = titleTags.First().Value;
            }


            var metaTags = _htmlHelper.GetElements(document, "meta");

            if (metaTags.Any())
            {
                var attributeValues = from metaTag in metaTags
                                      let attribute = _htmlHelper.GetAttribute(metaTag, "name")
                                      where attribute != null
                                      where attribute.Value == "description"
                                      select _htmlHelper.GetAttribute(metaTag, "content");
                if (attributeValues.Any())
                {
                    searchResultPreview.Description = attributeValues.First().Value;
                }
            }



            return searchResultPreview;
        }
    }
}
