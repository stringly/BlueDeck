using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Persistence.Repositories
{
    public class MemberDutyStatusRepository : Repository<DutyStatus>, IMemberDutyStatusRepository
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
            return GetAll().ToList().ConvertAll(x => new MemberDutyStatusSelectListItem { MemberDutyStatusId = System.Convert.ToInt32(x.DutyStatusId), MemberDutyStatusName = x.DutyStatusName });
        }

        public DutyStatus GetStatusById(int memberDutyStatus)
        {
            return ApplicationDbContext.DutyStatuses
                .Where(x => x.DutyStatusId == memberDutyStatus)
                .FirstOrDefault();
        }
        /// <summary>
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }

        public List<DutyStatus> GetDutyStatusesWithMemberCount()
        {
            return ApplicationDbContext.DutyStatuses.Include(x => x.Members).ToList();
        }
    }
}
