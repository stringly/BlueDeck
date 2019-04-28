using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.ViewModels
{
    public class ChangeEmployeeStatusModalViewComponentViewModel
    {        
        public int? MemberId { get; set; }
        [Required]
        [Display(Name = "Choose Duty Status:")]
        public int? DutyStatus { get; set; }
        public int? ParentComponentId { get; set; }
        public Member Member { get; set; }
        public List<MemberDutyStatusSelectListItem> StatusList { get; set; }

        public ChangeEmployeeStatusModalViewComponentViewModel()
        {
        }

        public ChangeEmployeeStatusModalViewComponentViewModel(Member member, List<MemberDutyStatusSelectListItem> statusList)
        {
            MemberId = member.MemberId;
            DutyStatus = member.DutyStatus.DutyStatusId;
            ParentComponentId = member.Position.ParentComponent.ComponentId;
            Member = member;
            StatusList = statusList;
        }
    }
}
