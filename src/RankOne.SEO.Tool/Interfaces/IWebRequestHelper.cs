using System.Net;

namespace RankOne.Interfaces
{
    public interface IWebRequestHelper
    {
        HttpStatusCode GetStatus(string url);
        bool IsActiveUrl(string url);
    }
}
