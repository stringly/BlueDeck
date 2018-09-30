using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrgChartDemo.Repositories
{
    // generic interface where TEntity is a Class
    public interface IRepository<TEntity> where TEntity : class
    {
        // Finding Objects
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        // Adding Objects
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        // Remove Objects
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
