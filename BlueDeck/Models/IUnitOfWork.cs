using System;
using BlueDeck.Models.Repositories;

namespace BlueDeck.Models
{
    /// <summary>
    /// An interface that represents the encapsulated Entity Repository Interfaces 
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IPositionRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IComponentRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRepository"/>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRankRepository"/> 
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IPositionRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Position Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IPositionRepository"/>
        IPositionRepository Positions { get; }

        /// <summary>
        /// Gets an <see cref="BlueDeck.Models.Repositories.IComponentRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Component Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IComponentRepository"/>
        IComponentRepository Components { get; }

        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IMemberRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Member Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRepository"/>
        IMemberRepository Members { get; }

        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IMemberRankRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRanks Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IPositionRepository"/>
        IMemberRankRepository MemberRanks { get; }

        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IMemberGenderRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberGender Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberGenderRepository"/>
        IMemberGenderRepository MemberGenders { get; }

        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IMemberRaceRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRaceRepository"/>
        IMemberRaceRepository MemberRaces { get; }

        /// <summary>
        /// Gets an <see cref="T:BlueDeck.Models.Repositories.IMemberDutyStatusRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberDutyStatusRepository"/>
        IMemberDutyStatusRepository MemberDutyStatus { get; }

        IMemberContactNumberRepository MemberContactNumbers { get; }

        IPhoneNumberTypeRepository PhoneNumberTypes { get; }

        IAppStatusRepository AppStatuses { get; }

        /// <summary>
        /// Saves changes made in the Unit of Work to ensure consistent updates
        /// </summary>
        /// <returns></returns>
        int Complete();
    }
}
