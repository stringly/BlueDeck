using System;
using OrgChartDemo.Models.Repositories;

namespace OrgChartDemo.Models
{
    /// <summary>
    /// An interface that represents the encapsulated Entity Repository Interfaces 
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository"/>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IComponentRepository"/>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository"/>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRankRepository"/> 
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IPositionRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Position Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository"/>
        IPositionRepository Positions { get; }

        /// <summary>
        /// Gets an <see cref="OrgChartDemo.Models.Repositories.IComponentRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Component Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IComponentRepository"/>
        IComponentRepository Components { get; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Member Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository"/>
        IMemberRepository Members { get; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberRankRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRanks Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository"/>
        IMemberRankRepository MemberRanks { get; }

        /// <summary>
        /// Saves changes made in the Unit of Work to ensure consistent updates
        /// </summary>
        /// <returns></returns>
        int Complete();
    }
}
