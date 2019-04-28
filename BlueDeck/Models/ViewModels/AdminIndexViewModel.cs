using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class AdminIndexViewModel
    {
        public IEnumerable<Member> GlobalAdmins { get; set; }
        public IEnumerable<Member> PendingAccounts { get; set; }
        
        public AdminIndexViewModel()
        {
            GlobalAdmins = new List<Member>();
            PendingAccounts = new List<Member>();
        }
    }
}
