namespace RankOne.Interfaces
{
    public interface ICacheHelper
    {
        bool Exists(string key);
        object GetValue(string key);
        void SetValue(string key, object value);
    }
}
