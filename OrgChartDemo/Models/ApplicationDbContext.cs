using Microsoft.EntityFrameworkCore;

namespace OrgChartDemo.Models {

    /// <summary>
    /// Entity Framework DbContext Class
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class ApplicationDbContext : DbContext {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> of <see cref="ApplicationDbContext"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }
        public ApplicationDbContext()
        {

        }
        /// <summary>
        /// Gets or sets the Components.
        /// </summary>
        /// <value>
        /// A <see cref="DbSet{TEntity}"/> of <see cref="Component"/>s.
        /// </value>
        public virtual DbSet<Component> Components { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// A <see cref="DbSet{TEntity}"/> of <see cref="Member"/>s
        /// </value>
        public virtual DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the Positions.
        /// </summary>
        /// <value>
        /// A <see cref="DbSet{TEntity}"/> of <see cref="Position"/>s
        /// </value>
        public virtual DbSet<Position> Positions { get; set; }
    }
}
