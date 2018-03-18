using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var imageTags = pageData.Document.GetElements("img");
            var imageTagCount = imageTags.Count();

            CheckImagesForAttribute(imageTags, imageTagCount, "alt");
            CheckImagesForAttribute(imageTags, imageTagCount, "title");

            if (!AnalyzeResult.ResultRules.Any())
            {
                AddResultRule("alt_and_title_tags_present", ResultType.Success);
            }
        }

        private void CheckImagesForAttribute(IEnumerable<HtmlNode> imageTags, int imageTagCount, string attributeName)
        {
            var imagesWithAttributeCount =
                imageTags.Count(x => x.GetAttribute(attributeName) != null && !string.IsNullOrWhiteSpace(x.GetAttribute(attributeName).Value));

            if (imageTagCount > imagesWithAttributeCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "missing_" + attributeName + "_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingAttribute = imageTagCount - imagesWithAttributeCount;
                resultRule.Tokens.Add(numberOfTagsMissingAttribute.ToString());
                AddResultRule(resultRule);
            }
        }
    }
}