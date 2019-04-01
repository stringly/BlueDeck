using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Persistence.Repositories;
using System.Linq;

namespace OrgChartDemo.Persistence
{
    /// <summary>
    /// An instance of <see cref="T:OrgChartDemo.Models.IUnitOfWork"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        //private readonly Member CurrentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">An <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/>.</param>
        public UnitOfWork(ApplicationDbContext context/*, IHttpContextAccessor httpContext*/)
        {
            _context = context;
            Positions = new PositionRepository(_context);
            Components = new ComponentRepository(_context);
            Members = new MemberRepository(_context);
            MemberRanks = new MemberRankRepository(_context);
            MemberGenders = new MemberGenderRepository(_context);
            MemberRaces = new MemberRaceRepository(_context);
            MemberDutyStatus = new MemberDutyStatusRepository(_context);
            MemberContactNumbers = new MemberContactNumberRepository(_context);
            PhoneNumberTypes = new PhoneNumberTypeRepository(_context);
            AppStatuses = new AppStatusRepository(_context);

        }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IPositionRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Position Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository" />
        public IPositionRepository Positions { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IComponentRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Component Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IComponentRepository" />
        public IComponentRepository Components { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the Member Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository" />
        public IMemberRepository Members { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberRankRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRanks Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository" />
        public IMemberRankRepository MemberRanks { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberGenderRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberGender Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberGenderRepository" />
        public IMemberGenderRepository MemberGenders { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberRaceRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRaceRepository" />
        public IMemberRaceRepository MemberRaces { get; private set; }

        /// <summary>
        /// Gets an <see cref="T:OrgChartDemo.Models.Repositories.IMemberDutyStatusRepository" />
        /// </summary>
        /// <value>
        /// The Interface representing the MemberRace Entity.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberDutyStatusRepository" />
        public IMemberDutyStatusRepository MemberDutyStatus { get; private set; }

        public IMemberContactNumberRepository MemberContactNumbers { get; private set; }

        public IPhoneNumberTypeRepository PhoneNumberTypes { get; private set; }

        public IAppStatusRepository AppStatuses { get; private set; }

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
