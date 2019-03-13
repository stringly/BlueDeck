using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<HomePageComponentGroup> ComponentGroups { get; set; }
        public string UserFirstName { get; set; }
        public string UserPositionName { get; set; }
        public int UserMemberId { get; set; }
        public int UserPositionId { get; set; }
        public string UserComponentName { get; set; }
        public int UserComponentId { get; set; }

        public HomePageViewModel(Member member)
        {
            UserFirstName = member.FirstName;
            UserPositionName = member.Position.Name;
            UserMemberId = member.MemberId;
            UserPositionId = member.Position.PositionId;
            UserComponentName = member.Position.ParentComponent.Name;
            UserComponentId = member.Position.ParentComponent.ComponentId;
            ComponentGroups = new List<HomePageComponentGroup>();

        }
    }
}
