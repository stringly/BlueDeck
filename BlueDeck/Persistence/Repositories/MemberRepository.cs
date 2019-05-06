using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BlueDeck.Models.APIModels;
using System.Threading.Tasks;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Member entity
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Member}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRepository" />
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Persistence.Repositories.MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="T:BlueDeck.Models.ApplicationDbContext"/>.</param>
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
        public IEnumerable<Member> GetMembersWithRank()
        {
            return ApplicationDbContext.Members
                .Include(x => x.Rank)
                .ToList();
        }
        public MemberIndexListViewModel GetMemberIndexListViewModel()
        {
            MemberIndexListViewModel vm = new MemberIndexListViewModel();
            vm.Members = ApplicationDbContext.MemberIndexViewModelMemberListItems.FromSql("EXECUTE Get_Member_Index_List").ToList();
            return vm;              
                                    
        }
        public AdminMemberIndexListViewModel GetAdminMemberIndexListViewModel()
        {
            AdminMemberIndexListViewModel vm = new AdminMemberIndexListViewModel();
            vm.Members = ApplicationDbContext.Members
                            .Include(x => x.AppStatus)
                            .Include(x => x.Position)
                                .ThenInclude(x => x.ParentComponent)
                            .Include(x => x.CurrentRoles)
                            .ToList()
                            .ConvertAll(x => new AdminMemberIndexViewModelListItem(x));
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
                .Include(x => x.Creator)
                .Include(x => x.LastModifiedBy)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)                
                .Include(x => x.DutyStatus)
                .Include(x => x.AppStatus)
                .Include(x => x.CurrentRoles)
                    .ThenInclude(x => x.RoleType)
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
            if (form.MemberId != null) // if MemberId is not null, we are editing an existing Member
            {
                    m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .Include(x => x.CurrentRoles)
                .FirstOrDefault(x => x.MemberId == form.MemberId);

                m.LastModifiedById = form.LastModifiedById;
                m.LastModified = Convert.ToDateTime(form.LastModified);
            }
            else
            {
                m = new Member();
                m.CurrentRoles = new List<Role>();

                m.LastModified = Convert.ToDateTime(form.LastModified);
                m.LastModifiedById = form.LastModifiedById;
                m.CreatorId = form.CreatedById;
                m.CreatedDate = Convert.ToDateTime(form.CreatedDate);

            }
            if (form.PositionId != null){ 
                m.PositionId = Convert.ToInt32(form.PositionId);
            }
            else if (m.PositionId == 0)
            {
                m.PositionId = 7; // 7 is the current ID of the Unassigned Position
            }
            m.RankId = Convert.ToInt32(form.MemberRank);
            m.GenderId = Convert.ToInt32(form.MemberGender);
            m.RaceId = Convert.ToInt32(form.MemberRace);
            m.DutyStatusId = Convert.ToInt32(form.DutyStatusId);
            m.AppStatusId = Convert.ToInt32(form.AppStatusId);
            m.Email = form.Email;
            m.FirstName = form.FirstName;
            m.IdNumber = form.IdNumber;
            m.MiddleName = form.MiddleName;
            m.LastName = form.LastName;
            m.LDAPName = form.LDAPName;

            
            foreach(ContactNumber n in form.ContactNumbers)
            {
                if (n.MemberContactNumberId != 0)
                {
                    if (n.ToDelete == true)
                    {
                        ContactNumber toRemove = ApplicationDbContext.ContactNumbers.Where(x => x.MemberContactNumberId == n.MemberContactNumberId).FirstOrDefault();

                        ApplicationDbContext.ContactNumbers.Remove(toRemove);
                    }
                    else
                    {
                        ContactNumber toUpdate = m.PhoneNumbers.FirstOrDefault(x => x.MemberContactNumberId == n.MemberContactNumberId);
                        toUpdate.PhoneNumber = n.PhoneNumber;
                        toUpdate.Type = ApplicationDbContext.PhoneNumberTypes.FirstOrDefault(x => x.PhoneNumberTypeId == n.Type.PhoneNumberTypeId);
                    }
                }
                else if (n.PhoneNumber != null)
                {
                    ContactNumber toAdd = new ContactNumber()
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
            switch (form.IsUser)
            {
                case true:
                    if(!m.CurrentRoles.Any(x => x.RoleType.RoleTypeName == "User"))
                    {
                            RoleType userRoleType = ApplicationDbContext.RoleTypes.First(x => x.RoleTypeName == "User");
                            Role r = new Role()
                            {
                                RoleType = userRoleType
                            };
                            m.CurrentRoles.Add(r);
                    }
                    break;
                case false:
                    Role userRole = m.CurrentRoles.FirstOrDefault(x => x.RoleType.RoleTypeName == "User");
                    if (userRole != null)
                    {
                        ApplicationDbContext.Roles.Remove(userRole);  
                    }
                    break;
            }
            // 
            switch (form.IsComponentAdmin)
            {
                case true:
                    if(!m.CurrentRoles.Any(x => x.RoleType.RoleTypeName == "ComponentAdmin"))
                    {
                            RoleType userRoleType = ApplicationDbContext.RoleTypes.First(x => x.RoleTypeName == "ComponentAdmin");
                            Role r = new Role()
                            {
                                RoleType = userRoleType
                            };
                            m.CurrentRoles.Add(r);
                    }
                    break;
                case false:
                    Role userRole = m.CurrentRoles.FirstOrDefault(x => x.RoleType.RoleTypeName == "ComponentAdmin");
                    if (userRole != null)
                    {
                        ApplicationDbContext.Roles.Remove(userRole);  
                    }
                    break;
            }
            switch (form.IsGlobalAdmin)
            {
                case true:
                    if(!m.CurrentRoles.Any(x => x.RoleType.RoleTypeName == "GlobalAdmin"))
                    {
                            RoleType userRoleType = ApplicationDbContext.RoleTypes.First(x => x.RoleTypeName == "GlobalAdmin");
                            Role r = new Role()
                            {
                                RoleType = userRoleType
                            };
                            m.CurrentRoles.Add(r);
                    }
                    break;
                case false:
                    Role userRole = m.CurrentRoles.FirstOrDefault(x => x.RoleType.RoleTypeName == "GlobalAdmin");
                    if (userRole != null)
                    {
                        ApplicationDbContext.Roles.Remove(userRole);    
                    }
                    break;
            }
        }
        public void Remove(int memberId)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .Include(x => x.CurrentRoles)
                .FirstOrDefault(x => x.MemberId == memberId);
            //ApplicationDbContext.ContactNumbers.RemoveRange(m.PhoneNumbers);
            //ApplicationDbContext.Roles.RemoveRange(m.CurrentRoles);
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

            List<HomePageComponentGroup> initial = components.ConvertAll(x => new HomePageComponentGroup(x));
            result.SetComponentOrder(initial);
            return result;
        }
        public int GetMemberParentComponentId(int memberid)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.Position).ThenInclude(x => x.ParentComponent)
                .FirstOrDefault(x => x.MemberId == memberid);
                
            return m.Position.ParentComponent.ComponentId;
                
                

        }
        public List<MemberSelectListItem> GetMembersUserCanEdit(int parentComponentId)
        {
            SqlParameter param1 = new SqlParameter("@ComponentId", parentComponentId);
            return ApplicationDbContext.GetMembersUserCanEdit.FromSql("dbo.Get_Members_User_Can_Edit @ComponentId", param1).ToList();
        }
        public List<Member> GetGlobalAdmins()
        {
            return ApplicationDbContext.Members
                .Include(x => x.CurrentRoles)
                    .ThenInclude(x => x.RoleType)
                .Where(x => x.CurrentRoles.Any(r => r.RoleType.RoleTypeName == "GlobalAdmin"))
                .ToList();
        }
        public List<Member> GetPendingAccounts()
        {
            return ApplicationDbContext.Members.Where(x => x.AppStatusId == 2).ToList();
        }
        public async Task<MemberApiResult> GetApiMember(int id)
        {
            var member = await ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.Race)
                .Include(x => x.Gender)
                .Include(x => x.DutyStatus)
                .Include(x => x.Rank)
                .FirstOrDefaultAsync(x => x.MemberId == id);
            if (member == null)
            {
                return null;
            }
            else
            {
                if (member.Position.IsManager)
                {
                    // start looking for manager in Member's position's ParentComponent
                }
                MemberApiResult result = new MemberApiResult(member);
                if (member.Position.IsManager)
                {
                    var supervisor = await FindNearestManager(Convert.ToInt32(member.Position.ParentComponent.ParentComponentId));
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }                    
                }
                else
                {
                    var supervisor = await FindNearestManager(member.Position.ParentComponentId);
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }  
                }                
                return result;
            }
            
        }
        private async Task<Member> FindNearestManager(int _componentId)
        {
            // attempt to locate a manager in the current component's positions
            Position p = await ApplicationDbContext.Positions
                .Where(x => x.ParentComponentId == _componentId && x.IsManager == true)
                .FirstOrDefaultAsync();

            // if no manager is found, retrieve parent
            if (p == null)
            {
                Component component = await ApplicationDbContext.Components.FindAsync(_componentId);
                if (component?.ParentComponentId != null)
                {                    
                    return await FindNearestManager(Convert.ToInt32(component.ParentComponentId));                    
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Member m = await ApplicationDbContext.Members
                .Where(x => x.PositionId == p.PositionId)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)
                .FirstOrDefaultAsync();
                return m;
            }                                    
        }        
        public IEnumerable<RoleType> GetMemberRoles()
        {
            return ApplicationDbContext.RoleTypes.ToList();
        }
    }
}
