using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An interface that encapsulates data store interactions common to all Domain Entity Interactions. 
    /// </summary>
    /// <remarks>
    /// This interface should not be implemented directly, but rather a Entity-specific interface that implements this interface should be created,
    /// and specific implementations of Repositories should be derived from Entity-specific interface 
    /// </remarks>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IPositionRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IComponentRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRankRepository"/>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the Entity with the given identifier.
        /// </summary>
        /// <param name="id">The identifier for the desired entity.</param>
        /// <returns>A <see cref="T:TEntity"/></returns>
        TEntity Get(int id);

        /// <summary>
        /// Gets Entities in the Repository
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerable{T}"/> of the Entity.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Finds the Entities that match the specified predicate.
        /// </summary>
        /// <remarks>
        /// This Method can accept LINQ Lambda Expression delegates
        /// </remarks>
        /// <param name="predicate">The predicate LINQ Lambda Expression delegate.</param>
        /// <returns>An <see cref="T:System.Collections.IEnumerable{T}"/> of the Entity.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds a Single Entity based on the predicate.
        /// </summary>
        /// <remarks>
        /// This Method can accept LINQ Lambda Expression delegates
        /// </remarks>
        /// <param name="predicate">The predicate LINQ Lambda Expression delegate.</param>
        /// <returns>A <see cref="T:TEntity"/></returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The Entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds a range of Entities.
        /// </summary>
        /// <param name="entities">An <see cref="T:IEnumerable{T}"/> of entities to add.</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The <see cref="T:TEntity:"/> to add.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes a range of <see cref="T:TEntity"/>.
        /// </summary>
        /// <param name="entities">An <see cref="T:IEnumerable{T}"/> of entities to remove.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}