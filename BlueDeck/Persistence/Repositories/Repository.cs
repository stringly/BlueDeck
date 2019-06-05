using Microsoft.EntityFrameworkCore;
using BlueDeck.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Represents CRUD actions common to DbContext entities
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity}" />
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// An <see cref="DbContext"/> context
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(DbContext context)
        {
            Context = context;            
        }

        /// <summary>
        /// Gets the Entity with the given identifier.
        /// </summary>
        /// <param name="id">The identifier for the desired entity.</param>
        /// <returns>
        /// A <see cref="TEntity" />
        /// </returns>
        public TEntity Get(int id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Gets Entities in the Repository
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> of the Entity.
        /// </returns>
        public IEnumerable<TEntity> GetAll()
        {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I didn't change it because I wanted the code to look like the videos. But feel free to change
            // this on your own.
            return Context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Finds the Entities that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate LINQ Lambda Expression delegate.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> of the Entity.
        /// </returns>
        /// <remarks>
        /// This Method can accept LINQ Lambda Expression delegates
        /// </remarks>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// Finds a Single Entity based on the predicate.
        /// </summary>
        /// <param name="predicate">The predicate LINQ Lambda Expression delegate.</param>
        /// <returns>
        /// A <see cref="TEntity" />
        /// </returns>
        /// <remarks>
        /// This Method can accept LINQ Lambda Expression delegates
        /// </remarks>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The Entity to add.</param>
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Adds a range of Entities.
        /// </summary>
        /// <param name="entities">An <see cref="IEnumerable{T}" /> of entities to add.</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity" /> to add.</param>
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Removes a range of <see cref="TEntity" />.
        /// </summary>
        /// <param name="entities">An <see cref="IEnumerable{T}" /> of entities to remove.</param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
