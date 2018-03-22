using System;
using Umbraco.Core.Models;

namespace RankOne.Tests.Mock
{
    public class PublishedPropertyMock : IPublishedProperty
    {
        public object DataValue
        {
            get
            {
                return Value;
            }
        }

        public bool HasValue
        {
            get
            {
                return Value != null;
            }
        }

        public string PropertyTypeAlias
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public object XPathValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}