using System.Collections.Generic;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models.ViewModels
{
    public class MemberIndexListViewModel
    {
        public IEnumerable<MemberIndexViewModelMemberListItem> Members { get; set; }
        public string MemberFirstNameSort { get; set; }
        public string MemberLastNameSort { get; set; }
        public string IdNumberSort { get; set; }
        public string PositionNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public MemberIndexListViewModel(List<Member> members)
        {
            Members = members.ConvertAll(x => new MemberIndexViewModelMemberListItem(x));
        }
    }
}
