using System;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An interface that represents the encapsulated Entity Repository Interfaces 
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <seealso cref="IPositionRepository"/>
    /// <seealso cref="IComponentRepository"/>
    /// <seealso cref="IMemberRepository"/>
    /// <seealso cref="IMemberRankRepository"/>
    /// <seealso cref="IMemberGenderRepository"/>
    /// <seealso cref="IMemberRaceRepository"/>
    /// <seealso cref="IMemberDutyStatusRepository"/>
    /// <seealso cref="IMemberContactNumberRepository"/>
    /// <seealso cref="IAppStatusRepository" />
    /// <seealso cref="IMemberRoleTypeRepository"/>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets an <see cref="IPositionRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Position Entity.
        /// </value>
        /// <seealso cref="IPositionRepository"/>
        IPositionRepository Positions { get; }

        /// <summary>
        /// Gets an <see cref="IComponentRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Component Entity.
        /// </value>
        /// <seealso cref="IComponentRepository"/>
        IComponentRepository Components { get; }

        /// <summary>
        /// Gets an <see cref="IMemberRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the Member Entity.
        /// </value>
        /// <seealso cref="IMemberRepository"/>
        IMemberRepository Members { get; }

        /// <summary>
        /// Gets an <see cref="IMemberRankRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRanks Entity.
        /// </value>
        /// <seealso cref="IMemberRankRepository"/>
        IMemberRankRepository MemberRanks { get; }

        /// <summary>
        /// Gets an <see cref="IMemberGenderRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberGender Entity.
        /// </value>
        /// <seealso cref="IMemberGenderRepository"/>
        IMemberGenderRepository MemberGenders { get; }

        /// <summary>
        /// Gets an <see cref="IMemberRaceRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="IMemberRaceRepository"/>
        IMemberRaceRepository MemberRaces { get; }

        /// <summary>
        /// Gets an <see cref="IMemberDutyStatusRepository"/>
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="IMemberDutyStatusRepository"/>
        IMemberDutyStatusRepository MemberDutyStatus { get; }
        /// <summary>
        /// Gets an <see cref="IMemberContactNumberRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberContactNumber Entity.
        /// </value>
        /// <seealso cref="IMemberContactNumberRepository" />
        IMemberContactNumberRepository MemberContactNumbers { get; }
        /// <summary>
        /// Gets an <see cref="IPhoneNumberTypeRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the PhoneNumberType Entity.
        /// </value>
        /// <seealso cref="IPhoneNumberTypeRepository" />
        IPhoneNumberTypeRepository PhoneNumberTypes { get; }
        /// <summary>
        /// Gets an <see cref="IAppStatusRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the AppStatus Entity.
        /// </value>
        /// <seealso cref="IAppStatusRepository" />
        IAppStatusRepository AppStatuses { get; }
        /// <summary>
        /// Gets an <see cref="IMemberRoleTypeRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the RoleType Entity.
        /// </value>
        /// <seealso cref="IMemberRoleTypeRepository" />
        IMemberRoleTypeRepository RoleTypes { get; }

        /// <summary>
        /// Saves changes made in the Unit of Work to ensure consistent updates
        /// </summary>
        /// <returns></returns>
        int Complete();
    }
}
