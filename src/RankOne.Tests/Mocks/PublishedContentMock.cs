using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace RankOne.Tests.Mocks
{
    public class PublishedContentMock : IPublishedContent
    {
        public object this[string alias]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IPublishedContent> Children
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IPublishedContent> ContentSet
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PublishedContentType ContentType { get; private set; }

        public DateTime CreateDate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CreatorId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CreatorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DocumentTypeAlias
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int DocumentTypeId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Id
        {
            get; set;
        }

        public bool IsDraft
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PublishedItemType ItemType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Level
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPublishedContent Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Path
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<IPublishedProperty> Properties
        {
            get; set;
        }

        public int SortOrder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TemplateId
        {
            get;
            set;
        }

        public DateTime UpdateDate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Url
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string UrlName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int WriterId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string WriterName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int GetIndex()
        {
            throw new NotImplementedException();
        }

        public IPublishedProperty GetProperty(string alias)
        {
            return Properties.SingleOrDefault(p => p.PropertyTypeAlias == alias);
        }

        public IPublishedProperty GetProperty(string alias, bool recurse)
        {
            return GetProperty(alias);
        }
    }
}