using RankOne.Interfaces;
using RankOne.Models;
using System.Web.Script.Serialization;

namespace RankOne.Serializers
{
    public class DashboardSettingsSerializer : JavaScriptSerializer, IDashboardSettingsSerializer
    {
        public string Serialize(DashboardSettings score)
        {
            return base.Serialize(score);
        }

        public DashboardSettings Deserialize(string input)
        {
            return Deserialize<DashboardSettings>(input);
        }
    }
}