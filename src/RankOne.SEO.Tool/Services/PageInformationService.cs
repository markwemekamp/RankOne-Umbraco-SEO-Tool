using System.Linq;
using HtmlAgilityPack;
using RankOne.Helpers;
using RankOne.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageInformationService
    {
        protected HtmlHelper HtmlHelper;

        public PageInformationService()
        {
            HtmlHelper = new HtmlHelper();
        }

        public PageInformation GetpageInformation(int id)
        {
            var pageInformation = new PageInformation();

            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var htmlObject = umbracoHelper.RenderTemplate(id);

            var html = htmlObject.ToHtmlString();

            var htmlParser = new HtmlDocument();
            htmlParser.LoadHtml(html);

            var headTag = HtmlHelper.GetElements(htmlParser.DocumentNode, "head");
            var titleTags = HtmlHelper.GetElements(headTag.First(), "title");

            if (titleTags.Any())
            {
                pageInformation.Title = titleTags.First().InnerText;
            }

            var metaTags = HtmlHelper.GetElements(htmlParser.DocumentNode, "meta");

            var attributeValues = from metaTag in metaTags
                                  let attribute = HtmlHelper.GetAttribute(metaTag, "name")
                                  where attribute != null
                                  where attribute.Value == "description"
                                  select HtmlHelper.GetAttribute(metaTag, "content");


            if (attributeValues.Any())
            {
                pageInformation.Description = attributeValues.First().Value;
            }

            return pageInformation;
        }
    }
}
