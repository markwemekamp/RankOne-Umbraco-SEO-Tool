using System;

namespace RankOne.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Summary : Attribute
    {
        public string Alias { get; set; }
        public int SortOrder { get; set; }
    }
}