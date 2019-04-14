using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.ViewModels
{
    public class MemberLineupViewComponentViewModel
    {
        public IEnumerable<MemberLineupItem> CurrentMembers { get; set; }
        public List<MemberSelectListItem> AvailableMembers { get; set; }
        public int? PositionId { get; set; }
        public int? SelectedMember { get; set; }

        public MemberLineupViewComponentViewModel()
        {
        }
        public MemberLineupViewComponentViewModel(Position _position, List<MemberSelectListItem> _members)
        {
            CurrentMembers = _position.Members.ConvertAll(x => new MemberLineupItem(x));
            PositionId = _position.PositionId;
            AvailableMembers = _members;
        }
    }
}
