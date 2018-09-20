using Microsoft.EntityFrameworkCore;

namespace OrgChartDemo.Models {

    public interface IContext {
        DbSet<Component> Components { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<Position> Positions { get; set; }
    }
    /// <summary>
    /// Entity Framework DbContext Class
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class ApplicationDbContext : DbContext, IContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Component> Components { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
