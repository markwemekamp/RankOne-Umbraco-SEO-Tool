using System;

namespace RankOne.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Summary : Attribute
    {
        public int SortOrder { get; set; }
    }
}
