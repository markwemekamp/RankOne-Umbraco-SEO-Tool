using System.Linq;

namespace RankOne.Helpers
{
    public class MinificationHelper
    {
        public bool IsMinified(string content)
        {
            var totalCharacters = content.Length;
            var lines = content.Count(x => x == '\n');
            var ratio = totalCharacters / lines;          // ratio characters per line
            return ratio < 200;
        }
    }
}
