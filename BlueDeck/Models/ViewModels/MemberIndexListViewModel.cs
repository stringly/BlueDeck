using System.Collections.Generic;
using System.Linq;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class MemberIndexListViewModel
    {
        public IEnumerable<MemberIndexViewModelMemberListItem> Members { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string MemberFirstNameSort { get; set; }
        public string MemberLastNameSort { get; set; }
        public string IdNumberSort { get; set; }
        public string PositionNameSort { get; set; }
        public string CruiserNumberSort { get; set;}
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public MemberIndexListViewModel()
        {
            Members = new List<MemberIndexViewModelMemberListItem>();
        }

        public MemberIndexListViewModel(List<Member> members)
        {
            Members = members.ConvertAll(x => new MemberIndexViewModelMemberListItem(x));
        }
    }
}
