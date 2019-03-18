using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Member}" />
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
        HomePageViewModel GetHomePageViewModelForMember(int memberId);
        int GetMemberParentComponentId(int memberid);
        List<MemberSelectListItem> GetMembersUserCanEdit(List<ComponentSelectListItem> canEditComponents);
    }
}
