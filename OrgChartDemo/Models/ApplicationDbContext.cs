using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models {

    /// <summary>
    /// Entity Framework DbContext Class
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    public class ApplicationDbContext : DbContext {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="T:Microsoft.EntityFrameWorkCore.DbContextOptions"/> of <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless Constructor
        /// </remarks>
        public ApplicationDbContext()
        {
        }
        /// <summary>
        /// Gets or sets the Components.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.Component"/>s.
        /// </value>
        public virtual DbSet<Component> Components { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.Member"/>s
        /// </value>
        public virtual DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the Positions.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.Position"/>s
        /// </value>
        public virtual DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Gets or sets the MemberRanks.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberRank"/>s
        /// </value>
        public virtual DbSet<MemberRank> MemberRanks { get; set; }

        /// <summary>
        /// Gets or sets the MemberRaces.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberRace"/>s
        /// </value>
        public virtual DbSet<MemberRace> MemberRace { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberGender"/>s
        /// </value>
        public virtual DbSet<MemberGender> MemberGender { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberDutyStatus"/>s
        /// </value>
        public virtual DbSet<MemberDutyStatus> DutyStatus { get; set; }

        public virtual DbSet<MemberContactNumber> ContactNumbers { get; set; }

        public virtual DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
    }
}
