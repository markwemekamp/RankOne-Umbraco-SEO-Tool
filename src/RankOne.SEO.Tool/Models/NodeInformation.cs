using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Umbraco.Core.Models;

namespace RankOne.Models
{
    [DataContract]
    public class NodeInformation
    {
        public NodeInformation(IPublishedContent node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Node = node;
            Id = node.Id;
            TemplateId = node.TemplateId;
            Name = node.Name;
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public IPublishedContent Node { get; private set; }

        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public int TemplateId { get;  private set; }
        [DataMember]
        public string Name { get; private set; }
    }
}