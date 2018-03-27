using HtmlAgilityPack;
using RankOne.Collections;

namespace RankOne.Interfaces
{
    public interface IWordCounter
    {
        WordOccurenceCollection GetKeywords(HtmlNode html);
    }
}