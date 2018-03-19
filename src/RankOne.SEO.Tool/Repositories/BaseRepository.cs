using RankOne.Interfaces;
using System.Collections.Generic;

namespace RankOne.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private IDatabaseService<T> _databaseService;

        public string TableName => _databaseService.TableName;

        public bool TableExists => _databaseService.TableExists;

        public BaseRepository(IDatabaseService<T> databaseService)
        {
            _databaseService = databaseService;
        }

        public virtual T GetById(int id)
        {
            return _databaseService.GetById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _databaseService.GetAll();
        }

        public virtual T Insert(T dbEntity)
        {
            return _databaseService.Insert(dbEntity);
        }

        public virtual T Update(T dbEntity)
        {
            return _databaseService.Update(dbEntity);
        }

        public virtual void Delete(T dbEntity)
        {
            _databaseService.Delete(dbEntity);
        }
    }
}