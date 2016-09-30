using System.Collections.Generic;
using System.Linq;

namespace RankOne.Models
{
    public class HiearchyNode
    {
        public NodeInformation NodeInformation { get; set; }
        public string FocusKeyword { get; set; }
        public PageScore PageScore { get; set; }
        public IEnumerable<HiearchyNode> Children { get; set; }

        public HiearchyNode()
        {
            Children = new List<HiearchyNode>();
        }

        public bool HasChildrenWithTemplate
        {
            get { return NodeInformation.TemplateId > 0 || Children.Any(x => x.NodeInformation.TemplateId > 0 || x.HasChildrenWithTemplate); }
        }
    }
}
