namespace RankOne.Interfaces
{
    public interface IJavascriptSerializer<T>
    {
        string Serialize(T score);

        T Deserialize(string input);
    }
}