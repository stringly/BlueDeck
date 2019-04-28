using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.ViewModels
{
    public class ReassignEmployeeModalViewComponentViewModel
    {
        public IEnumerable<PositionSelectListItem> Positions { get; set; }
        public Member Member { get; set; }
        [Required]
        [Display(Name = "Available Positions:")]
        public int PositionId { get; set; }
        public int MemberId { get; set; }
        public int SelectedComponentId { get; set; }

        public ReassignEmployeeModalViewComponentViewModel()
        {
        }

        public ReassignEmployeeModalViewComponentViewModel(Member m, IEnumerable<PositionSelectListItem> p, int s)
        {
            Positions = p;
            Member = m;
            MemberId = m.MemberId;
            SelectedComponentId = s;
        }
    }
}
