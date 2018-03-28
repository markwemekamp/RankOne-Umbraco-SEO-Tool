using System;

namespace RankOne.Models.Exceptions
{
    public abstract class ElementException : Exception
    {
        public string ElementName { get; internal set; }

        public ElementException(string elementName)
        {
            if (elementName == null) throw new ArgumentNullException(nameof(elementName));

            ElementName = elementName;
        }
    }
}