using Microsoft.EntityFrameworkCore;

namespace OrgChartDemo.Models {

    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Component> Components { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Position> Positions { get; set; }


    }
}
