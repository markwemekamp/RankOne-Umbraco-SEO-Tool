using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEO.Umbraco.Extensions.Models
{
    public class SeoNode
    {
        public string Url { get; set; }

        public List<SeoNode> Children { get; set; }

        public SeoNode()
        {
            Children = new List<SeoNode>();
        }
    }
}
