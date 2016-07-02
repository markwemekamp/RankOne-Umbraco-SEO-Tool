using System.Collections.Generic;

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
    }
}
