using System;

namespace RankOne.Models.Exceptions
{
    public abstract class ElementException : Exception
    {
        public string ElementName { get; set; }

        public ElementException(string elementName)
        {
            ElementName = elementName;
        }
    }
}
