using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class ComponentDemographicTableViewComponentViewModel
    {
        public Component Component { get; set; }
        public List<MemberGenderSelectListItem> Genders { get; set; }
        public List<MemberRaceSelectListItem> Races { get; set; }
        public List<MemberRankSelectListItem> Ranks { get; set; }

        public ComponentDemographicTableViewComponentViewModel(
            Component _c,
            List<MemberGenderSelectListItem> _g, 
            List<MemberRaceSelectListItem> _rc,
            List<MemberRankSelectListItem> _rk)
        {
            Component = _c;
            Genders = _g;
            Races = _rc;
            Ranks = _rk;        
        }
    }
}
