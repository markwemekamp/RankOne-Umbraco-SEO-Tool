using System.Net;
using System.Text.RegularExpressions;

namespace RankOne.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string UrlFriendly(this string text)
        {
            text = text.RemoveAccents().ToLower();
            text = WebUtility.HtmlDecode(text);
            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            text = htmlRegex.Replace(text, string.Empty);
            // invalid chars
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space
            text = Regex.Replace(text, @"\s+", " ").Trim();
            // cut and trim
            text = text.Substring(0, text.Length <= 45 ? text.Length : 45).Trim();
            text = Regex.Replace(text, @"\s", "-"); // hyphens
            return text;
        }

        public static string RemoveAccents(this string text)
        {
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string Simplify(this string text)
        {
            text = text.RemoveAccents().ToLower();
            text = WebUtility.HtmlDecode(text);

            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            text = htmlRegex.Replace(text, string.Empty);

            var rgx = new Regex("[^a-z0-9-_]");
            text = rgx.Replace(text, string.Empty);

            return text;
        }
    }
}