using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class AssignMemberToPositionViewModel
    {
        public Position Position { get; set; }
        public int? PositionId { get; set; }
        public int? AssignedMember { get; set; }
        public List<MemberSelectListItem> Members { get; set; }

        
    }
}
