namespace RankOne.Interfaces
{
    public interface IMinificationHelper
    {
        bool IsMinified(string content, int densityRatio = 200);
    }
}