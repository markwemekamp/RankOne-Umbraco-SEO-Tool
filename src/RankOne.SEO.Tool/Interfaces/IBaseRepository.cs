using System.Collections.Generic;
using Umbraco.Core.Persistence;

namespace RankOne.Interfaces
{
    public interface IBaseRepository<T>
    {
        bool TableExists { get; }

        T GetById(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllByQuery(Sql query);

        T Insert(T dbEntity);

        T Update(T dbEntity);

        void Delete(T dbEntity);
    }
}