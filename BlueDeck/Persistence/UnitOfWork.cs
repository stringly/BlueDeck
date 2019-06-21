using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Persistence.Repositories;

namespace BlueDeck.Persistence
{
    /// <summary>
    /// An instance of <see cref="IUnitOfWork"/>
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">An <see cref="ApplicationDbContext"/>.</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Positions = new PositionRepository(_context);
            Components = new ComponentRepository(_context);
            Members = new MemberRepository(_context);
            Vehicles = new VehicleRepository(_context);
            VehicleModels = new VehicleModelRepository(_context);
            VehicleManufacturers = new VehicleManufacturerRepository(_context);
            MemberRanks = new MemberRankRepository(_context);
            MemberGenders = new MemberGenderRepository(_context);
            MemberRaces = new MemberRaceRepository(_context);
            MemberDutyStatus = new MemberDutyStatusRepository(_context);
            MemberContactNumbers = new MemberContactNumberRepository(_context);
            PhoneNumberTypes = new PhoneNumberTypeRepository(_context);
            AppStatuses = new AppStatusRepository(_context);
            RoleTypes = new MemberRoleTypeRepository(_context);

        }

        /// <summary>
        /// Gets an <see cref="IPositionRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Position Entity.
        /// </value>
        /// <seealso cref="IPositionRepository" />
        public IPositionRepository Positions { get; private set; }

        /// <summary>
        /// Gets an <see cref="IComponentRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Component Entity.
        /// </value>
        /// <seealso cref="IComponentRepository" />
        public IComponentRepository Components { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Member Entity.
        /// </value>
        /// <seealso cref="IMemberRepository" />
        public IMemberRepository Members { get; private set; }

        /// <summary>
        /// Gets the vehicles.
        /// </summary>
        /// <value>
        /// The vehicles.
        /// </value>
        public IVehicleRepository Vehicles { get; private set; }

        /// <summary>
        /// Gets the vehicle models.
        /// </summary>
        /// <value>
        /// The vehicle models.
        /// </value>
        public IVehicleModelRepository VehicleModels { get; private set; }

        /// <summary>
        /// Gets the vehicle manufacturers.
        /// </summary>
        /// <value>
        /// The vehicle manufacturers.
        /// </value>
        public IVehicleManufacturerRepository VehicleManufacturers { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberRankRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRanks Entity.
        /// </value>
        /// <seealso cref="IMemberRankRepository" />
        public IMemberRankRepository MemberRanks { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberGenderRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberGender Entity.
        /// </value>
        /// <seealso cref="IMemberGenderRepository" />
        public IMemberGenderRepository MemberGenders { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberRaceRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="IMemberRaceRepository" />
        public IMemberRaceRepository MemberRaces { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberDutyStatusRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="IMemberDutyStatusRepository" />
        public IMemberDutyStatusRepository MemberDutyStatus { get; private set; }

        /// <summary>
        /// Gets an <see cref="IMemberContactNumberRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberContactNumber Entity.
        /// </value>
        /// <seealso cref="IMemberContactNumberRepository" />
        public IMemberContactNumberRepository MemberContactNumbers { get; private set; }
        /// <summary>
        /// Gets an <see cref="IPhoneNumberTypeRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the PhoneNumberType Entity.
        /// </value>
        /// <seealso cref="IPhoneNumberTypeRepository" />
        public IPhoneNumberTypeRepository PhoneNumberTypes { get; private set; }
        /// <summary>
        /// Gets an <see cref="IAppStatusRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the AppStatus Entity.
        /// </value>
        /// <seealso cref="IAppStatusRepository" />
        public IAppStatusRepository AppStatuses { get; private set; }
        /// <summary>
        /// Gets an <see cref="IMemberRoleTypeRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the RoleType Entity.
        /// </value>
        /// <seealso cref="IMemberRoleTypeRepository" />
        public IMemberRoleTypeRepository RoleTypes {get; private set;}

        /// <summary>
        /// Saves changes made in the Unit of Work to ensure consistent updates
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
