using System.Collections.Generic;
using System.Linq;

namespace RankOne.Models
{
    public class PageScoreNode
    {
        public NodeInformation NodeInformation { get; set; }
        public string FocusKeyword { get; set; }
        public PageScore PageScore { get; set; }
        public IEnumerable<PageScoreNode> Children { get; set; }

        public PageScoreNode()
        {
            Children = new List<PageScoreNode>();
        }

        public bool HasChildrenWithTemplate
        {
            get { return Children.Any(x => x.NodeInformation.TemplateId > 0 || x.HasChildrenWithTemplate); }
        }
    }
}