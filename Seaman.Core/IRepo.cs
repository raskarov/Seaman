using System;

namespace Seaman.Core
{
    public interface IRepo<TEntity,TKey> where TEntity : IEntity<TKey>
    {
        void Remove(TKey id);
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Find(TKey id);
    }


    public interface IRepo<TEntity> : IRepo<TEntity, Int32> where TEntity : IEntity<Int32>
    {


    }
}