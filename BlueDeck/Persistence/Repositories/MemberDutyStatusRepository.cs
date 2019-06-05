using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Repository that Controls CRUD operations for the <see cref="DutyStatus"/> entity.
    /// </summary>
    /// <seealso cref="Repositories.Repository{Models.DutyStatus}" />
    /// <seealso cref="IMemberDutyStatusRepository" />
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

        /// <summary>
        /// Gets the Duty Status with the given identifier.
        /// </summary>
        /// <param name="memberDutyStatus">The DutyStatusId of the desired Duty Status.</param>
        /// <returns>A <see cref="DutyStatus"/></returns>
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
        /// <summary>
        /// Gets the a list of all current duty statuses with member count.
        /// </summary>
        /// <returns>A <see cref="List{DutyStatus}"/> of all current Duty Status types</returns>
        public List<DutyStatus> GetDutyStatusesWithMemberCount()
        {
            return ApplicationDbContext.DutyStatuses.Include(x => x.Members).ToList();
        }

        /// <summary>
        /// Gets a duty status with member count.
        /// </summary>
        /// <param name="id">The DutyStatusId of the desired Duty Status.</param>
        /// <returns>A <see cref="DutyStatus"/></returns>
        public DutyStatus GetDutyStatusWithMemberCount(int id)
        {
            return ApplicationDbContext.DutyStatuses.Include(x => x.Members).First(x => x.DutyStatusId == id);
        }

        /// <summary>
        /// Removes the Duty Status with the specified identifier.
        /// </summary>
        /// <remarks>
        /// If the Duty Status being deleted has Members assigned to it, this method will try to reassign them the to "Full Duty" status, or, failing that, assign them to DutyStatusId = 1.
        /// This is to prevent orphaning Members. The current calling methods are supposed to prevent allowing this method to be called on Duty Statuses with active Members.
        /// </remarks>
        /// <param name="id">The DutyStatusId of the Duty Status to remove.</param>
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
