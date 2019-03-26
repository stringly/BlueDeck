using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Member entity
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Member}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository" />
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/>.</param>
        public MemberRepository(ApplicationDbContext context)
         : base(context)
        {
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
        /// Gets the members with positions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Member> GetMembersWithPositions()
        {
            return ApplicationDbContext.Members
                .Include(c => c.Rank)
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.DutyStatus)
                .Include(c => c.Position)
                .ThenInclude(c => c.ParentComponent)
                .ToList();
        }

        public MemberIndexListViewModel GetMemberIndexListViewModel()
        {
            MemberIndexListViewModel vm = new MemberIndexListViewModel();
            vm.Members = ApplicationDbContext.MemberIndexViewModelMemberListItems.FromSql("EXECUTE Get_Member_Index_List").ToList();
            return vm;              
                                    
        }

        public Member GetMemberWithPosition(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)                
                .FirstOrDefault();
        }

        public Member GetHomePageMember(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)
                .FirstOrDefault();
        }
        public Member GetMemberWithDemographicsAndDutyStatus(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)                
                .Include(x => x.DutyStatus)                    
                .Include(x => x.Gender)                    
                .Include(x => x.Race)                    
                .Include(x => x.Rank)                    
                .FirstOrDefault();
        }

        public IEnumerable<MemberSelectListItem> GetAllMemberSelectListItems()
        {
            return ApplicationDbContext.Members
                .Include(x => x.Rank)
                .OrderByDescending(x => x.IdNumber)
                .ToList().ConvertAll(x => new MemberSelectListItem { MemberId = x.MemberId, MemberName = x.GetTitleName() });
        }

        public void UpdateMember(MemberAddEditViewModel form)
        {
            Member m;
            if (form.MemberId != null)
            {
                    m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .FirstOrDefault(x => x.MemberId == form.MemberId);
            }
            else
            {
                m = new Member();
            }
            if (form.PositionId != null){ 
                m.Position = ApplicationDbContext.Positions.FirstOrDefault(x => x.PositionId == form.PositionId);
            }
            m.Rank = ApplicationDbContext.MemberRanks.SingleOrDefault(x => x.RankId == form.MemberRank);
            m.Gender = ApplicationDbContext.MemberGender.SingleOrDefault(x => x.GenderId == form.MemberGender);
            m.Race = ApplicationDbContext.MemberRace.SingleOrDefault(x => x.MemberRaceId == form.MemberRace);
            m.DutyStatus = ApplicationDbContext.DutyStatus.SingleOrDefault(x => x.DutyStatusId == form.DutyStatusId);
            m.Email = form.Email;
            m.FirstName = form.FirstName;
            m.IdNumber = form.IdNumber;
            m.MiddleName = form.MiddleName;
            m.LastName = form.LastName;
            foreach(MemberContactNumber n in form.ContactNumbers)
            {
                if (n.MemberContactNumberId != 0)
                {
                    if (n.ToDelete == true)
                    {
                        MemberContactNumber toRemove = ApplicationDbContext.ContactNumbers.Where(x => x.MemberContactNumberId == n.MemberContactNumberId).FirstOrDefault();

                        ApplicationDbContext.ContactNumbers.Remove(toRemove);
                    }
                    else
                    {
                        MemberContactNumber toUpdate = m.PhoneNumbers.FirstOrDefault(x => x.MemberContactNumberId == n.MemberContactNumberId);
                        toUpdate.PhoneNumber = n.PhoneNumber;
                        toUpdate.Type = ApplicationDbContext.PhoneNumberTypes.FirstOrDefault(x => x.PhoneNumberTypeId == n.Type.PhoneNumberTypeId);
                    }
                }
                else if (n.PhoneNumber != null)
                {
                    MemberContactNumber toAdd = new MemberContactNumber()
                    {
                        Member = m,
                        PhoneNumber = n.PhoneNumber,
                        Type = ApplicationDbContext.PhoneNumberTypes.FirstOrDefault(x => x.PhoneNumberTypeId == n.Type.PhoneNumberTypeId)
                    };
                    m.PhoneNumbers.Add(toAdd);
                }
            }
            if (form.MemberId == null)
            {
                ApplicationDbContext.Members.Add(m);
            }
        }

        public void Remove(int memberId)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .FirstOrDefault(x => x.MemberId == memberId);
            ApplicationDbContext.ContactNumbers.RemoveRange(m.PhoneNumbers);
            ApplicationDbContext.Members.Remove(m);
        }

        public Member GetMemberWithRoles(string LDAPName)
        {
            return ApplicationDbContext.Members
                .Include(x => x.Position).ThenInclude(x => x.ParentComponent)
                .Include(x => x.CurrentRoles).ThenInclude(x => x.RoleType)
                .Include(x => x.Gender)
                .Include(x => x.Rank)                
                .FirstOrDefault(x => x.LDAPName == LDAPName);
        }

        /// <summary>
        /// Gets the home page view model for member.
        /// </summary>
        /// <remarks>
        /// This is an attempt to try and apply some better SOLID principals to the
        /// way I build these viewModels...
        /// </remarks>
        /// <param name="memberId">The member identifier.</param>
        /// <returns></returns>
        public HomePageViewModel GetHomePageViewModelForMember(int memberId)
        {
            Member currentUser = ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.Rank)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)                
                .FirstOrDefault();
            HomePageViewModel result = new HomePageViewModel(currentUser);
            SqlParameter param1 = new SqlParameter("@ComponentId", currentUser.Position.ParentComponent.ComponentId);

            List<Component> components = ApplicationDbContext.Components.FromSql("dbo.GetComponentAndChildrenDemo @ComponentId", param1).ToList();
            ApplicationDbContext.Set<Position>().Where(x => components.Contains(x.ParentComponent))
                .Include(y => y.Members).ThenInclude(z => z.Rank)
                .Include(y => y.Members).ThenInclude(z => z.Gender)
                .Include(y => y.Members).ThenInclude(x => x.Race)
                .Include(y => y.Members).ThenInclude(x => x.DutyStatus)
                .Include(y => y.Members).ThenInclude(x => x.PhoneNumbers)
                .Load();
            foreach(Component c in components.OrderBy(x => x.LineupPosition))
            {
                HomePageComponentGroup grp = new HomePageComponentGroup(c);
                result.ComponentGroups.Add(grp);
            }
            return result;
        }

        public int GetMemberParentComponentId(int memberid)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.Position).ThenInclude(x => x.ParentComponent)
                .FirstOrDefault(x => x.MemberId == memberid);
                
            return m.Position.ParentComponent.ComponentId;
                
                

        }

        public List<MemberSelectListItem> GetMembersUserCanEdit(List<ComponentSelectListItem> canEditComponents)
        {
            List<MemberSelectListItem> result = new List<MemberSelectListItem>();
            foreach (ComponentSelectListItem c in canEditComponents)
            {
                List<Member> members = ApplicationDbContext.Members.Where(x => x.Position.ParentComponent.ComponentId == c.Id).ToList();
                result.AddRange(members.ConvertAll(x => new MemberSelectListItem(x)));
            }
            return result;
        }

    }
}
