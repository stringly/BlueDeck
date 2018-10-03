﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRank entity.
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Types.MemberRank}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRankRepository" />
    public class MemberRankRepository : Repository<MemberRank>, IMemberRankRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRankRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public MemberRankRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberRankSelectListItem" />s.
        /// </summary>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.MemberRankSelectListItem}" />
        /// </returns>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        public List<MemberRankSelectListItem> GetMemberRankSelectListItems()
        {            
            return GetAll().ToList().ConvertAll(x => new MemberRankSelectListItem { MemberRankId = x.RankId, RankName = x.RankFullName });
        }
    }

   
}
