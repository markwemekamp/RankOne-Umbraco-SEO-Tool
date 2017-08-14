using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface ITableNameHelper<T> where T : BaseDatabaseObject
    {
        string GetTableName();
    }
}