using RankOne.Interfaces;

namespace RankOne.Models
{
    public class Option : IOption
    {
        public string Key
        {
            get; set;
        }

        public string Value
        {
            get; set;
        }
    }
}