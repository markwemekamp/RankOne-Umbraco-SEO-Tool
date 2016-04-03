using System.Xml.Linq;

namespace RankOne.Business.Models
{
    public class HtmlResult
    {
        public string Url { get; set; }
        public XDocument Document { get; internal set; }
        public string Html { get; set; }
        public long ServerResponseTime { get; set; }
        public int Size { get; internal set; }
    }
}
