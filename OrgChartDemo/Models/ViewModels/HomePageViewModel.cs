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
        public Member CurrentUser {get;set;}

        public HomePageViewModel(Member member)
        {
            CurrentUser = member;
            ComponentGroups = new List<HomePageComponentGroup>();

        }
    }
}
