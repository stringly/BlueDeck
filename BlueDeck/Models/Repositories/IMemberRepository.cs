using BlueDeck.Models.APIModels;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Member}" />
    public interface IMemberRepository : IRepository<Member>
    {
        IEnumerable<Member> GetMembersWithPositions();
        Member GetMemberWithPosition(int memberId);
        Member GetMemberWithDemographicsAndDutyStatus(int memberId);
        IEnumerable<MemberSelectListItem> GetAllMemberSelectListItems();
        void UpdateMember(MemberAddEditViewModel vm);

        /// <summary>
        /// Removes the specified member identifier.
        /// </summary>
        /// <remarks>
        /// This is an override of the IRepository's Remove method. This override removes all MemberContact entries 
        /// when removing a Member.
        /// </remarks>
        /// <param name="memberId">The member identifier.</param>
        void Remove(int memberId);
        Member GetMemberWithRoles(string LDAPName);
        MemberIndexListViewModel GetMemberIndexListViewModel();
        AdminMemberIndexListViewModel GetAdminMemberIndexListViewModel();
        HomePageViewModel GetHomePageViewModelForMember(int memberId);
        int GetMemberParentComponentId(int memberid);
        List<MemberSelectListItem> GetMembersUserCanEdit(int parentComponentId);
        List<Member> GetGlobalAdmins();
        List<Member> GetPendingAccounts();
        Task<MemberApiResult> GetApiMember(int id);
    }
}
