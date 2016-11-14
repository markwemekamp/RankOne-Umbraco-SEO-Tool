using System.Net;
using System.Text.RegularExpressions;

namespace RankOne.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string Alias(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string ConvertToSimpleWord(this string word)
        {
            word = word.ToLower();
            word = WebUtility.HtmlDecode(word);

            var htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
            word = htmlRegex.Replace(word, string.Empty);

            var rgx = new Regex("[^a-zA-Z0-9-]");
            word = rgx.Replace(word, string.Empty);

            return word;
        }
    }
}
