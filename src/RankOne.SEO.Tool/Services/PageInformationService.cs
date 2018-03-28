using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;
using System.Web;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageInformationService : IPageInformationService
    {
        private readonly ITypedPublishedContentQuery _typedPublishedContentQuery;
        private readonly ITemplateHelper _templateHelper;

        public PageInformationService() : this(RankOneContext.Instance)
        { }

        public PageInformationService(IRankOneContext rankOneContext) : this(rankOneContext.TypedPublishedContentQuery.Value, rankOneContext.TemplateHelper.Value)
        { }

        public PageInformationService(ITypedPublishedContentQuery typedPublishedContentQuery, ITemplateHelper templateHelper)
        {
            if (typedPublishedContentQuery == null) throw new ArgumentNullException(nameof(typedPublishedContentQuery));
            if (templateHelper == null) throw new ArgumentNullException(nameof(templateHelper));

            _typedPublishedContentQuery = typedPublishedContentQuery;
            _templateHelper = templateHelper;
        }

        public PageInformation GetpageInformation(int id)
        {
            var pageInformation = new PageInformation();

            var content = _typedPublishedContentQuery.TypedContent(id);
            var html = _templateHelper.GetNodeHtml(content);

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