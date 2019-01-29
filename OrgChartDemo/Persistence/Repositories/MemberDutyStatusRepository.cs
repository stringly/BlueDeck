using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Persistence.Repositories
{
    public class MemberDutyStatusRepository : Repository<MemberDutyStatus>, IMemberDutyStatusRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Models.Persistence.MemberDutyStatusRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public MemberDutyStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberGenderSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.MemberGenderSelectListItem}" />
        /// </returns>
        public List<MemberDutyStatusSelectListItem> GetMemberDutyStatusSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberDutyStatusSelectListItem { MemberDutyStatusId = x.DutyStatusId, MemberDutyStatusName = x.DutyStatusName });
        }
    }
}
