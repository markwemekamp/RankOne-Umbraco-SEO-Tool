using System.Net;

namespace RankOne.Interfaces
{
    public interface IHttpWebRequestFactory
    {
        HttpWebRequest Create(string url);
    }
}