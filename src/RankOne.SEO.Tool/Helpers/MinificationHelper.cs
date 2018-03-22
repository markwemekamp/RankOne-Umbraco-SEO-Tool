using RankOne.Interfaces;
using System;

namespace RankOne.Helpers
{
    public class MinificationHelper : IMinificationHelper
    {
        public bool IsMinified(string content, int densityRatio = 200)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var totalCharacters = content.Length;
            var lines = content.Split('\n').Length;
            var ratio = totalCharacters / lines;          // ratio characters per line
            return ratio > densityRatio;
        }
    }
}