using System.Collections.Generic;

namespace RankOne.Models
{
    public class ResultRule
    {
        public string Type { get; set; }

        public string Alias { get; set; }

        public List<string> Tokens { get; set; }

        public ResultRule()
        {
            Tokens = new List<string>();
        }
    }
}