using System;

namespace RankOne.Interfaces
{
    public interface IUrlHelper
    {
        string GetFullPath(string path, Uri url);

        string GetContent(string fullPath);
    }
}