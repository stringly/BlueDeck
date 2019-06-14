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
    /// <seealso cref="Repository{Member}" />
    /// <seealso cref="IMemberRepository" />
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/>.</param>
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
                .Include(x => x.TempPosition)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)
                .Include(x => x.CurrentRoles)
                    .ThenInclude(x => x.RoleType)
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
            }
            else
            {
                m = new Member();
                m.CurrentRoles = new List<Role>();
                m.CreatorId = form.CreatedById;
                m.CreatedDate = Convert.ToDateTime(form.CreatedDate);

            }

            /* there are a few things that can happen at this point:
                1. This is a new Member, which means there are no Roles
                    - Check for a Position/Temp Position assignment in the form data
                        -if no Position/Temp Position, then
                            1. Position should be set to the "Unassigned"
                            2. Temp Position should be null
                2. This is an existing Member, which means there MAY be roles
                    - Check for a Position/Temp Position assignment in the form data
                        - if no Position/Temp Position, assume the member is to be reassigned to the General Pool
                            - if so, remove Component Admin Role (Leave Global Admin Role... global should be de-coupled from assignment
                        - if a Position/Temp Position exists and Neither has ComponentAdmin privilege, check for and Remove the ComponentAdmin role from the Member's CurrentRoles
                        - if a Position/Temp Position exists and Either has ComponentAdmin privilge, check for and, if necessary, add the Component Admin role to the Member's Current Roles.

            An easy way to do this might be by simply switching the form's "checkbox" property corresponding to the Component Admin role in these cases.
            
            */
            if (form.PositionId != null){ 
                m.PositionId = Convert.ToInt32(form.PositionId);
                if (m.AppStatusId == 3 || form.AppStatusId == 3) // I only want to add Roles to Active Accounts or Accounts being activated.
                {
                    Position formAssignedPosition = ApplicationDbContext.Positions.Find(form.PositionId);
                    Position formAssignedTempPosition = ApplicationDbContext.Positions.Find(form.TempPositionId); // how will this behave if null?
                    if (formAssignedPosition.IsManager == true || formAssignedPosition.IsAssistantManager == true || formAssignedTempPosition?.IsManager == true || formAssignedTempPosition?.IsAssistantManager == true )
                    {
                        // these positions require ComponentAdmin Privilege
                        form.IsComponentAdmin = true;
                    }
                    else
                    {
                        form.IsComponentAdmin = false;
                    }
                }
            }
            else if (m.PositionId == 0)
            {
                Position unassigned = ApplicationDbContext.Positions.FirstOrDefault(x => x.Name == "Unassigned");
                if (unassigned != null)
                {
                    m.Position = unassigned;
                }                
                form.IsComponentAdmin = false;
            }



            m.TempPositionId = form.TempPositionId;
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
            m.PayrollID = form.PayrollID;
            m.HireDate = form.HireDate;
            m.OrgPositionNumber = form.OrgPositionNumber;
            m.LastModified = Convert.ToDateTime(form.LastModified);
            m.LastModifiedById = form.LastModifiedById;
            // remove ALL roles from non-active accounts
            // at this point, the repo Member m data and the form data should match, so I can check the repo Member
            AppStatus activeStatus = ApplicationDbContext.ApplicationStatuses.FirstOrDefault(x => x.StatusName == "Active");
            
            if(m.AppStatusId != activeStatus?.AppStatusId)
            {
                 // these will override the checkboxes on the form.
                 form.IsUser = false;
                 form.IsComponentAdmin = false;
            }
            else
            {
                form.IsUser = true;
            }
            
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

        /// <summary>
        /// Removes the Member with the specified Identifier.
        /// </summary>
        /// <param name="memberId">The MemberId of the Member to remove.</param>
        /// <remarks>
        /// This is an override of the IRepository's Remove method. This override removes all MemberContact entries
        /// when removing a Member.
        /// </remarks>
        // TODO: Mark removed Entities as inactive instead of deleting?
        public void Remove(int memberId)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .Include(x => x.CurrentRoles)
                .FirstOrDefault(x => x.MemberId == memberId);
            ApplicationDbContext.Members.Remove(m);
        }

        /// <summary>
        /// Gets a specific Member with their roles via LDAP Name
        /// </summary>
        /// <param name="LDAPName">The Windows LDAP Name of the Member.</param>
        /// <returns>The <see cref="Member"/> with the given LDAP Name</returns>
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
            HomePageViewModel result = new HomePageViewModel(currentUser,
                ApplicationDbContext.Components.ToList().ConvertAll(x => new ComponentSelectListItem(x)),
                ApplicationDbContext.Genders.ToList().ConvertAll(x => new MemberGenderSelectListItem(x)),
                ApplicationDbContext.Races.ToList().ConvertAll(x => new MemberRaceSelectListItem(x)),
                ApplicationDbContext.Ranks.ToList().ConvertAll(x => new MemberRankSelectListItem(x)));
            SqlParameter param1 = new SqlParameter("@ComponentId", currentUser.Position.ParentComponent.ComponentId);

            List<Component> components = ApplicationDbContext.Components.FromSql("dbo.GetComponentAndChildrenDemo @ComponentId", param1).ToList();
            ApplicationDbContext.Set<Position>().Where(x => components.Contains(x.ParentComponent))
                .Include(y => y.Members).ThenInclude(z => z.Rank)
                .Include(y => y.Members).ThenInclude(z => z.Gender)
                .Include(y => y.Members).ThenInclude(x => x.Race)
                .Include(y => y.Members).ThenInclude(x => x.DutyStatus)
                .Include(y => y.Members).ThenInclude(x => x.PhoneNumbers)
                .Include(y => y.TempMembers).ThenInclude(x => x.Position)
                    .ThenInclude(z => z.ParentComponent)
                .Load();

            List<HomePageComponentGroup> initial = components.ConvertAll(x => new HomePageComponentGroup(x));
            result.SetComponentOrder(initial);
            result.GetExceptionToDutyMembers();
            return result;
        }
        public int GetMemberParentComponentId(int memberid)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.Position).ThenInclude(x => x.ParentComponent)
                .FirstOrDefault(x => x.MemberId == memberid);
                
            return m.Position.ParentComponent.ComponentId;
        }

        public void ReassignMemberAndSetRole(int MemberToReassignId, int newPositionId, bool IsTDY = false)
        {
            Member memberToReassign = ApplicationDbContext.Members
                .Where(x => x.MemberId == MemberToReassignId)
                .Include(x => x.CurrentRoles)
                .Include(x => x.Position)
                .Include(x => x.TempPosition)
                .FirstOrDefault();
            int activeAccountStatusId = ApplicationDbContext.ApplicationStatuses.FirstOrDefault(x => x.StatusName == "Active")?.AppStatusId ?? 0;
            if (memberToReassign != null) // ensure the member exists
            {
                Position newPosition = ApplicationDbContext.Positions.Find(newPositionId);
                if (newPosition != null) // ensure the new Position exists
                {
                    if (newPosition.IsAssistantManager == true || newPosition.IsManager == true) // check if the new Position is managerial
                    {
                         // if the new Position is managerial AND the Member DOES NOT have the ComponentAdmin Role AND the member's Account is active
                        if (!memberToReassign.IsComponentAdmin() && memberToReassign.AppStatusId == activeAccountStatusId)
                        {
                            // assign the Member to the Component Admin Role
                            Role componentAdminRole = new Role();
                            RoleType componentAdminRoleType = ApplicationDbContext.RoleTypes.Where(x => x.RoleTypeName == "ComponentAdmin").FirstOrDefault();
                            componentAdminRole.RoleType = componentAdminRoleType;
                            memberToReassign.CurrentRoles.Add(componentAdminRole);
                        }
                    }
                    else // if the new Position is not managerial
                    {
                        if (memberToReassign.IsComponentAdmin()) // check if the member has the ComponentAdmin Role
                        {
                            Role componentAdminRole = memberToReassign.CurrentRoles.Where(x => x.RoleType.RoleTypeName == "ComponentAdmin").FirstOrDefault();
                            // if role is present, determine if we are assigning them TDY, so we can check inherited permissions
                            if (IsTDY)
                            {
                                // if the new Position is TDY, then either the new Position or the Member's Primary position must be Manager/Assistant, or we remove the role.
                                if (newPosition.IsManager == false && newPosition.IsManager == false && memberToReassign.Position.IsManager == false && memberToReassign.Position.IsAssistantManager == false)
                                {
                                    memberToReassign.CurrentRoles.Remove(componentAdminRole);                                    
                                }
                            }
                            else
                            {
                                // if this is not a TDY, then either the reassigned member's Temp Position or the new Position must have Manager/Assistant
                                if (newPosition.IsManager == false && newPosition.IsManager == false && (memberToReassign.TempPosition == null || (memberToReassign.TempPosition.IsManager == false && memberToReassign.TempPosition.IsAssistantManager == false)))
                                {
                                    memberToReassign.CurrentRoles.Remove(componentAdminRole);                                    
                                }
                            }
                        }
                    }
                    // finally, make the actual reassignment
                    if (IsTDY)
                    {
                        memberToReassign.TempPosition = newPosition;
                    }
                    else
                    {
                        memberToReassign.Position = newPosition;
                    }
                }
            }
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
            int pendingAccountStatusId = ApplicationDbContext.ApplicationStatuses.FirstOrDefault(x => x.StatusName == "Pending")?.AppStatusId ?? 0;
            return ApplicationDbContext.Members.Where(x => x.AppStatusId == pendingAccountStatusId).ToList();
        }
        public async Task<MemberApiResult> GetApiMemberByBlueDeckId(int id)
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
                    var supervisor = await FindNearestManagerForComponentId(Convert.ToInt32(member.Position.ParentComponent.ParentComponentId));
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }                    
                }
                else
                {
                    var supervisor = await FindNearestManagerForComponentId(member.Position.ParentComponentId);
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }  
                }                
                return result;
            }
            
        }
        public async Task<MemberApiResult> GetApiMemberByOrgId(string id)
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
                .FirstOrDefaultAsync(x => x.IdNumber == id);
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
                    var supervisor = await FindNearestManagerForComponentId(Convert.ToInt32(member.Position.ParentComponent.ParentComponentId));
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }                    
                }
                else
                {
                    var supervisor = await FindNearestManagerForComponentId(member.Position.ParentComponentId);
                    if (supervisor != null)
                    {
                        result.Supervisor = new SubMemberApiResult(supervisor);
                    }  
                }                
                return result;
            }
        }
        public async Task<Member> FindNearestManagerForComponentId(int _componentId)
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
                    return await FindNearestManagerForComponentId(Convert.ToInt32(component.ParentComponentId));                    
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
        public async Task<Member> FindNearestAssistantManagerOrManagerForComponentId(int _componentId)
        {
            // attempt to locate a manager in the current component's positions
            Position p = await ApplicationDbContext.Positions
                .Where(x => x.ParentComponentId == _componentId && x.IsAssistantManager == true)
                .FirstOrDefaultAsync();

            // if no assistant Manager, try to find Primary Manager
            if (p == null)
            {
                p = await ApplicationDbContext.Positions
                    .Where(x => x.ParentComponentId == _componentId && x.IsManager == true)
                    .FirstOrDefaultAsync();
            }
            // if no primary manager is found, retrieve parent and recurse
            if (p == null)
            {
                Component component = await ApplicationDbContext.Components.FindAsync(_componentId);
                if (component?.ParentComponentId != null)
                {                    
                    return await FindNearestAssistantManagerOrManagerForComponentId(Convert.ToInt32(component.ParentComponentId));                    
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
        public async Task<Member> FindNearestManagerForMemberId(int memberid)
        {
            // retrieve the member
            Member m = await ApplicationDbContext.Members
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.MemberId == memberid);
            // determine if the member is the supervisor... if so, we need to move up one component
            if (m.Position.IsManager)
            {
                // re-fetch the Member's position to include the ParentComponent
                Position p = await ApplicationDbContext.Positions
                    .Include(x => x.ParentComponent)                        
                    .Where(x => x.PositionId == m.PositionId)
                    .FirstOrDefaultAsync();
                // start the parsing from the Parent Component of the Member's Current Component
                return await FindNearestManagerForComponentId((Int32)p.ParentComponent.ParentComponentId);
            }
            else
            {
                return await FindNearestManagerForComponentId(m.Position.ParentComponentId);
            }
            
        }
        public async Task<Member> FindNearestAssistantManagerOrManagerForMemberId(int memberid)
        {
            // The challenge here is:
            // 1. If the Member is the Manager, we need to start recursion from the Member's Position's Parent Component's Parent Component
            // 2. If the Member is the Assistant Manager, we need to start with the current component, but 

            // retrieve the member
            Member m = await ApplicationDbContext.Members
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.MemberId == memberid);
            if (m.Position.IsManager)
            {
                // re-fetch the Member's position to include the ParentComponent
                Position p = await ApplicationDbContext.Positions
                    .Include(x => x.ParentComponent)                        
                    .Where(x => x.PositionId == m.PositionId)
                    .FirstOrDefaultAsync();

                return await FindNearestAssistantManagerOrManagerForComponentId((Int32)p.ParentComponent.ParentComponentId);
            }
            else if (m.Position.IsAssistantManager)
            {
                // if the member in question is an Assistant, we want to try and retrive the Primary Manager from the Component
                Position primaryManager = await ApplicationDbContext.Positions
                    .Include(x => x.Members).ThenInclude(x => x.Rank)
                    .Include(x => x.Members).ThenInclude(x => x.Gender)
                    .Include(x => x.Members).ThenInclude(x => x.Race)
                    .Include(x => x.Members).ThenInclude(x => x.PhoneNumbers).ThenInclude(x => x.Type)
                    .Include(x => x.Members).ThenInclude(x => x.DutyStatus)
                    .Include(x => x.TempMembers).ThenInclude(x => x.Rank)
                    .Include(x => x.TempMembers).ThenInclude(x => x.Gender)
                    .Include(x => x.TempMembers).ThenInclude(x => x.Race)
                    .Include(x => x.TempMembers).ThenInclude(x => x.PhoneNumbers).ThenInclude(x => x.Type)
                    .Include(x => x.TempMembers).ThenInclude(x => x.DutyStatus)
                    .Where(x => x.ParentComponentId == m.Position.ParentComponentId && x.IsManager == true)
                    .FirstOrDefaultAsync();
                if (primaryManager != null)
                {
                    if (primaryManager.Members.Count != 0)
                    {
                        return primaryManager.Members.First();
                    }
                    else if (primaryManager.TempMembers.Count != 0)
                    {
                        return primaryManager.TempMembers.First();
                    }                    
                }
                // re-fetch the Member's position to include the ParentComponent
                Position p = await ApplicationDbContext.Positions
                    .Include(x => x.ParentComponent)                        
                    .Where(x => x.PositionId == m.PositionId)
                    .FirstOrDefaultAsync();

                return await FindNearestAssistantManagerOrManagerForComponentId((Int32)p.ParentComponent.ParentComponentId);
            }
            return await FindNearestAssistantManagerOrManagerForComponentId(m.Position.ParentComponentId);
        }

        public async Task<List<MemberListAPIListItem>> GetSubordinateMemberApiMemberForBlueDeckId(int id)
        {
            List<MemberListAPIListItem> result = new List<MemberListAPIListItem>();
            Member currentMember = ApplicationDbContext.Members
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Where(x => x.MemberId == id)
                .FirstOrDefault();

            if(currentMember == null || (!currentMember.Position.IsManager && !currentMember.Position.IsAssistantManager))
            {
                return result;
            }
            else
            {
                SqlParameter param1 = new SqlParameter("@ComponentId", currentMember.Position.ParentComponent.ComponentId);
                List<Component> components = ApplicationDbContext.Components.FromSql("dbo.GetComponentAndChildrenDemo @ComponentId", param1).ToList();
                ApplicationDbContext.Set<Position>().Where(x => components.Contains(x.ParentComponent))
                    .Include(y => y.Members).ThenInclude(z => z.Rank)
                    .Include(y => y.TempMembers).ThenInclude(x => x.Rank)                        
                    .Load();
                foreach (Component c in components)
                {
                    foreach(Position p in c.Positions)
                    {
                        foreach(Member m in p.Members)
                        {
                            result.Add(new MemberListAPIListItem(m));
                        }
                        foreach(Member m in p.TempMembers)
                        {
                            result.Add(new MemberListAPIListItem(m));
                        }
                    }                    
                }
                return result;
            }

        }
        public IEnumerable<RoleType> GetMemberRoles()
        {
            return ApplicationDbContext.RoleTypes.ToList();
        }
    }
}
