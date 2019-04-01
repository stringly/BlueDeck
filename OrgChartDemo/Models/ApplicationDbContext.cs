﻿using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;

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
        public virtual DbSet<Rank> Ranks { get; set; }

        /// <summary>
        /// Gets or sets the MemberRaces.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberRace"/>s
        /// </value>
        public virtual DbSet<Race> Races { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberGender"/>s
        /// </value>
        public virtual DbSet<Gender> Genders { get; set; }

        /// <summary>
        /// Gets or sets the MemberGenders.
        /// </summary>
        /// <value>
        /// A <see cref="T:Microsoft.EntityFrameWorkCore.DbSet{TEntity}"/> of <see cref="T:OrgChartDemo.Models.MemberDutyStatus"/>s
        /// </value>
        public virtual DbSet<DutyStatus> DutyStatuses { get; set; }

        public virtual DbSet<AppStatus> ApplicationStatuses {get;set;}

        public virtual DbSet<ContactNumber> ContactNumbers { get; set; }

        public virtual DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }


        public DbQuery<MemberIndexViewModelMemberListItem> MemberIndexViewModelMemberListItems { get; set; }
        public DbQuery<ComponentSelectListItem> GetChildComponentsForComponentId { get; set; }
        public DbQuery<PositionSelectListItem> GetPositionsUserCanEdit { get; set; }
        public DbQuery<MemberSelectListItem> GetMembersUserCanEdit { get; set; }


    }
}
