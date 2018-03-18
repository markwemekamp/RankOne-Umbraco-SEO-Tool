using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Analyzers.Keywords
{
    public class KeywordUrlAnalyzer : BaseAnalyzer
    {
        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var url = new Uri(pageData.Url);

            if (url.AbsolutePath == "" || url.AbsolutePath == "/")
            {
                AddResultRule("root_node", ResultType.Success);
            }
            else
            {
                var keywordUrl = pageData.Focuskeyword.UrlFriendly();
                if (url.AbsolutePath.Contains(keywordUrl))
                {
                    AddResultRule("url_contains_keyword", ResultType.Success);
                }
                else
                {
                    AddResultRule("url_doesnt_contain_keyword", ResultType.Hint);
                }
            }
        }
    }
}