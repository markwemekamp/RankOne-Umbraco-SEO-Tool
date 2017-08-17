using RankOne.Interfaces;
using RankOne.Models;
using System.Web.Script.Serialization;

namespace RankOne.Serializers
{
    public class PageScoreSerializer : JavaScriptSerializer, IPageScoreSerializer
    {
        public string Serialize(PageScore score)
        {
            return base.Serialize(score);
        }

        public PageScore Deserialize(string input)
        {
            return Deserialize<PageScore>(input);
        }
    }
}