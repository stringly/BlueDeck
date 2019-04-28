using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class AdminMemberIndexViewModelListItem
    {
        public int MemberId { get; set; }        
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "ID#")]
        public string IdNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Current Assignment")]        
        public string PositionName { get; set; }
        [Display(Name = "Account Status")]
        public string AccountState {get;set;}
        public int? AccountStateId {get;set;}
        [Display(Name = "User")]
        public bool IsUser {get;set;}
        [Display(Name = "Component Admin")]
        public bool IsComponentAdmin {get;set;}
        [Display(Name = "Global Admin")]
        public bool IsGlobalAdmin {get;set;}
        public int PositionId { get; set; }
        public string ParentComponentName { get; set; }
        public int ParentComponentId { get; set; }

        public AdminMemberIndexViewModelListItem()
        {

        }
        public AdminMemberIndexViewModelListItem(Member m)
        {
            MemberId = m.MemberId;
            FirstName = m.FirstName;
            LastName = m.LastName;
            IdNumber = m.IdNumber;
            Email = m.Email;
            PositionName = m.Position.Name;
            PositionId = m.Position.PositionId;
            ParentComponentName = m.Position?.ParentComponent?.Name ?? "None";
            ParentComponentId = m.Position?.ParentComponent?.ComponentId ?? 0;
            AccountState = m.AppStatus.StatusName;
            AccountStateId = m?.AppStatus?.AppStatusId;
            IsUser = m?.CurrentRoles?.Any(x => x.RoleType.RoleTypeId == 3) ?? false;
            IsComponentAdmin = m?.CurrentRoles?.Any(x => x.RoleType.RoleTypeId == 2) ?? false;
            IsGlobalAdmin = m?.CurrentRoles?.Any(x => x.RoleType.RoleTypeId == 1) ?? false;
        }
    }
}
