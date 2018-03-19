using Newtonsoft.Json;
using System;
using Umbraco.Core.Models;

namespace RankOne.Models
{
    public class NodeInformation
    {
        public NodeInformation(IPublishedContent node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Node = node;
        }

        [JsonIgnore]
        public IPublishedContent Node { get; private set; }

        public int Id { get { return Node.Id; } }
        public int TemplateId { get { return Node.TemplateId; } }
        public string Name { get { return Node.Name; } }
    }
}