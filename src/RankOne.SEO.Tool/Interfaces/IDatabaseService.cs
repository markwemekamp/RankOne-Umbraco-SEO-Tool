using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IDatabaseRepository<T>
    {
        string TableName { get; }
        bool TableExists { get; }

        void CreateTable();

        T GetById(int id);

        IEnumerable<T> GetAll();

        T Insert(T dbEntity);

        T Update(T dbEntity);

        void Delete(T dbEntity);
    }
}