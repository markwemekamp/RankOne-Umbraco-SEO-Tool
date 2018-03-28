namespace RankOne.Models.Exceptions
{
    public class MultipleElementsFoundException : ElementException
    {
        public MultipleElementsFoundException(string element) : base(element)
        {
        }
    }
}