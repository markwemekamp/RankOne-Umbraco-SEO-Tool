using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IBaseRepository<T>
    {
        string TableName { get; }

        bool TableExists { get; }

        T GetById(int id);

        IEnumerable<T> GetAll();

        T Insert(T dbEntity);

        T Update(T dbEntity);

        void Delete(T dbEntity);
    }
}