using RankOne.Interfaces;

namespace RankOne.Helpers
{
    public class MinificationHelper : IMinificationHelper
    {
        public bool IsMinified(string content)
        {
            var totalCharacters = content.Length;
            var lines = content.Split('\n').Length;
            var ratio = totalCharacters / lines;          // ratio characters per line
            return ratio < 200;
        }
    }
}