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
                    portSegment = string.Format(":{0}", url.Port);
                }

                path = string.Format("{0}://{1}{3}{2}", url.Scheme, url.Host, path, portSegment);

            }

            return path;
        }
    }
}
