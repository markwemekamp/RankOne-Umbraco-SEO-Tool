using RankOne.Interfaces;
using System;
using System.Net;

namespace RankOne.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        public string GetFullPath(string path, Uri url)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (url == null) throw new ArgumentNullException(nameof(url));

            if (IsLocalLink(path))
            {
                var portSegment = "";
                if (url.Port > 0 && url.Port != 80)
                {
                    portSegment = $":{url.Port}";
                }

                path = $"{url.Scheme}://{url.Host}{portSegment}{path}";
            }

            return path;
        }

        public bool IsLocalLink(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            return path.StartsWith("/") || path.StartsWith("./") || path.StartsWith("../");
        }

        public string GetContent(string fullPath)
        {
            if (fullPath == null) throw new ArgumentNullException(nameof(fullPath));

            try
            {
                var webClient = new WebClient();
                return webClient.DownloadString(fullPath);
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }
}