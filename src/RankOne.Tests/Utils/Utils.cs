using System;

namespace RankOne.Tests
{
    public static class Utils
    {
        public static string GenerateString(int length)
        {
            if (length < 1) throw new ArgumentException(nameof(length));

            var output = "";
            for (int i = 0; i < length; i++) { output += "a"; }
            return output;
        }
    }
}