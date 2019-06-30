using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class MemberIndexViewModelMemberListItem
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
        public int PositionId { get; set; }
        public string ParentComponentName { get; set; }
        public int ParentComponentId { get; set; }
        [Display(Name = "Assigned Vehicle")]
        public string CruiserNumber { get; set;}
        public int? AssignedVehicleId { get; set;}

        public MemberIndexViewModelMemberListItem()
        {

        }
        public MemberIndexViewModelMemberListItem(Member m)
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
            CruiserNumber = m?.AssignedVehicle?.CruiserNumber ?? "No Cruiser";
            AssignedVehicleId = m?.AssignedVehicle?.VehicleId;
        }
    }
}
