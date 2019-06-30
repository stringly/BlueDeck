using Microsoft.EntityFrameworkCore;
using BlueDeck.Models.Types;
using BlueDeck.Models.Enums;


namespace BlueDeck.Models {

    /// <summary>
    /// Entity Framework DbContext Class
    /// </summary>
    /// <seealso cref="DbContext" />
    public class ApplicationDbContext : DbContext {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> of <see cref="ApplicationDbContext"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
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
        /// A DbSet of <see cref="Component"/>s.
        /// </value>
        public virtual DbSet<Component> Components { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Member"/>s
        /// </value>
        public virtual DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the Positions.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Position"/>s
        /// </value>
        public virtual DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Vehicle"/>
        /// </value>
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Gets or sets the vehicle models.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="VehicleModel"/>
        /// </value>
        public virtual DbSet<VehicleModel> VehicleModels { get; set;}

        /// <summary>
        /// Gets or sets the vehicle manufacturers.
        /// </summary>
        /// <value>
        /// A SbSet of <see cref="VehicleManufacturer">
        /// </value>
        public virtual DbSet<VehicleManufacturer> VehicleManufacturers { get; set; }

        /// <summary>
        /// Gets or sets the MemberRanks.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Rank"/>s
        /// </value>
        public virtual DbSet<Rank> Ranks { get; set; }

        /// <summary>
        /// Gets or sets the MemberRaces.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Race"/>s
        /// </value>
        public virtual DbSet<Race> Races { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Gender"/>s
        /// </value>
        public virtual DbSet<Gender> Genders { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="DutyStatus"/>s
        /// </value>
        public virtual DbSet<DutyStatus> DutyStatuses { get; set; }

        /// <summary>
        /// Gets or sets the application statuses.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="AppStatus"/>
        /// </value>
        public virtual DbSet<AppStatus> ApplicationStatuses {get;set;}

        /// <summary>
        /// Gets or sets the contact numbers.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="ContactNumber"/>
        /// </value>
        public virtual DbSet<ContactNumber> ContactNumbers { get; set; }

        /// <summary>
        /// Gets or sets the phone number types.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="PhoneNumberType"/>
        /// </value>
        public virtual DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="Role"/>
        /// </value>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the role types.
        /// </summary>
        /// <value>
        /// A DbSet of <see cref="RoleType"/>
        /// </value>
        public virtual DbSet<RoleType> RoleTypes { get; set; }


        /// <summary>
        /// Gets or sets the member index view model member list items.
        /// </summary>
        /// <value>
        /// The member index view model member list items.
        /// </value>
        public DbQuery<MemberIndexViewModelMemberListItem> MemberIndexViewModelMemberListItems { get; set; }

        /// <summary>
        /// Gets or sets the get child components for component identifier.
        /// </summary>
        /// <value>
        /// The get child components for component identifier.
        /// </value>
        public DbQuery<ComponentSelectListItem> GetChildComponentsForComponentId { get; set; }

        /// <summary>
        /// Gets or sets the get positions user can edit.
        /// </summary>
        /// <value>
        /// The get positions user can edit.
        /// </value>
        public DbQuery<PositionSelectListItem> GetPositionsUserCanEdit { get; set; }

        /// <summary>
        /// Gets or sets the get members user can edit.
        /// </summary>
        /// <value>
        /// The get members user can edit.
        /// </value>
        public DbQuery<MemberSelectListItem> GetMembersUserCanEdit { get; set; }

        /// <summary>
        /// Gets or sets the get vehicles user can edit.
        /// </summary>
        /// <value>
        /// The get vehicles user can edit.
        /// </value>
        public DbQuery<VehicleSelectListItem> GetVehiclesUserCanEdit { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasOne(p => p.Position)
                .WithMany(m => m.Members)
                .HasForeignKey(p => p.PositionId);
            modelBuilder.Entity<Member>()
                .HasOne(p => p.TempPosition)
                .WithMany(m => m.TempMembers)
                .HasForeignKey(p => p.TempPositionId);
            //modelBuilder.Entity<Member>()
            //    .HasOptional(m => m.AssignedVehicle)
            //    .WithOne(v => v.AssignedToMember)
            //    .HasForeignKey<Vehicle>(v => v.AssignedToMemberId);
            modelBuilder.Entity<Member>()
                .HasOne(c => c.Creator)
                .WithMany(m => m.CreatedMembers)
                .HasForeignKey(m => m.CreatorId);
            modelBuilder.Entity<Member>()
                .HasOne(c => c.LastModifiedBy)
                .WithMany(m => m.LastModifiedMembers)
                .HasForeignKey(m => m.LastModifiedById);
            modelBuilder.Entity<Position>()
                .HasOne(c => c.Creator)
                .WithMany(m => m.CreatedPositions)
                .HasForeignKey(p => p.CreatorId);
            modelBuilder.Entity<Position>()
                .HasOne(c => c.LastModifiedBy)
                .WithMany(m => m.LastModifiedPositions)
                .HasForeignKey(p => p.LastModifiedById);
            modelBuilder.Entity<Component>()
                .HasOne(c => c.Creator)
                .WithMany(m => m.CreatedComponents)
                .HasForeignKey(p => p.CreatorId);
            modelBuilder.Entity<Component>()
                .HasOne(c => c.LastModifiedBy)
                .WithMany(m => m.LastModifiedComponents)
                .HasForeignKey(p => p.LastModifiedById);
            modelBuilder.Entity<Component>()
                .HasOne(c => c.ParentComponent)
                .WithMany(c => c.ChildComponents)
                .HasForeignKey(c => c.ParentComponentId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Model)
                .WithMany(v => v.Vehicles)
                .HasForeignKey(v => v.ModelId);            
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.AssignedToPosition)
                .WithMany(v => v.AssignedVehicles)
                .HasForeignKey(v => v.AssignedToPositionId);
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.AssignedToComponent)
                .WithMany(v => v.AssignedVehicles)
                .HasForeignKey(v => v.AssignedToComponentId);
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.Manufacturer)
                .WithMany(vm => vm.Models)
                .HasForeignKey(vm => vm.ManufacturerId);
            modelBuilder.Entity<Role>()
                .HasOne(r => r.RoleType)
                .WithMany(r => r.CurrentRoles)
                .HasForeignKey(r => r.RoleTypeId);
            modelBuilder.Entity<Role>()
                .HasOne(m => m.Member)
                .WithMany(m => m.CurrentRoles)
                .HasForeignKey(m => m.MemberId);
        }
    }
}
