using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;
using System.Web;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageInformationService : IPageInformationService
    {
        public PageInformation GetpageInformation(int id)
        {
            var pageInformation = new PageInformation();

            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var content = umbracoHelper.TypedContent(id);
            var htmlObject = umbracoHelper.RenderTemplate(id);

            var html = htmlObject.ToHtmlString();

            var htmlParser = new HtmlDocument();
            htmlParser.LoadHtml(HttpUtility.HtmlDecode(html));

            var headTag = htmlParser.DocumentNode.GetElements("head");

            if (headTag.Any())
            {
                var titleTags = headTag.First().GetElements("title");

                if (titleTags.Any())
                {
                    pageInformation.Title = titleTags.First().InnerText;
                }
            }

            var metaTags = htmlParser.DocumentNode.GetElements("meta");

            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "description"
                                  select metaTag.GetAttribute("content");

            if (attributeValues.Any())
            {
                pageInformation.Description = attributeValues.First().Value;
            }
            pageInformation.Url = content.UrlWithDomain();

            return pageInformation;
        }
    }
}