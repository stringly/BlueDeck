using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class HomePageMemberSearchResultViewComponentViewModel
    {
        public List<HomePageViewModelMemberListItem> Members {get; set;}

        public HomePageMemberSearchResultViewComponentViewModel(List<Member> members)
        {
            Members = members.ConvertAll(x => new HomePageViewModelMemberListItem(x));
        }
    }
}
