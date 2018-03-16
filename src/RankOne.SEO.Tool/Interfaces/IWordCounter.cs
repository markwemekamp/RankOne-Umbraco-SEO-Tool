using RankOne.Collections;
using RankOne.Models;
using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IWordCounter
    {
        IEnumerable<KeyValuePair<string, int>> GetKeywords(string text);

        IEnumerable<KeyValuePair<string, int>> GetKeywords(HtmlResult html);

        WordOccurenceCollection CountOccurencesForText(string textBlockText);
    }
}