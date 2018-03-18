namespace RankOne.Models.Exceptions
{
    public class NoElementFoundException : ElementException
    {
        public NoElementFoundException(string element) : base(element)
        {
        }
    }
}
