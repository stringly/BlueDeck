using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;

namespace BlueDeck.Persistence.Repositories
{
    public class MemberDutyStatusRepository : Repository<DutyStatus>, IMemberDutyStatusRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Models.Persistence.MemberDutyStatusRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:BlueDeck.Models.ApplicationDbContext"/></param>
        public MemberDutyStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberGenderSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{BlueDeck.Models.Types.MemberGenderSelectListItem}" />
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

        public void Remove(int id)
        {
            DutyStatus fullDuty = ApplicationDbContext.DutyStatuses.FirstOrDefault(x => x.IsExceptionToNormalDuty == false);
            List<Member> MembersInStatus = ApplicationDbContext.Members.Where(x => x.DutyStatusId == id).ToList();
            foreach(Member m in MembersInStatus)
            {
                m.DutyStatusId = fullDuty.DutyStatusId ?? 1;
            }
            DutyStatus statusToRemove = ApplicationDbContext.DutyStatuses.Find(id);
            if (statusToRemove != null){
                ApplicationDbContext.DutyStatuses.Remove(statusToRemove);
            }
            
        }
    }
}
