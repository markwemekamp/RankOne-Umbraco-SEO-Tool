using RankOne.Interfaces;
using System;

namespace RankOne.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        private bool IsLocalLink(string path)
        {
            return path.StartsWith("/");
        }

        public string GetFullPath(string path, Uri url)
        {
            if (IsLocalLink(path))
            {
                var portSegment = "";
                if (url.Port > 0)
                {
                    portSegment = $":{url.Port}";
                }

                path = $"{url.Scheme}://{url.Host}{portSegment}{path}";

            }

            return path;
        }
    }
}
