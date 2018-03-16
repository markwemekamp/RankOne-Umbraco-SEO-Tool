namespace RankOne.Interfaces
{
    public interface IByteSizeHelper
    {
        int GetByteSize(string htmlString);

        string GetSizeSuffix(int byteCount);
    }
}