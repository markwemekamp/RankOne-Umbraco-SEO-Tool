using System.Collections.Generic;

namespace RankOne.Business.Models
{
    public class ResultRule
    {
        public string Type { get; set; }

        public string Code { get; set; }

        public List<string> Tokens { get; set; }

        public ResultRule()
        {
            Tokens = new List<string>();
        }
    }
}
