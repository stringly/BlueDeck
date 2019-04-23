using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class AdminPositionIndexViewModelMemberListItem
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string JobTitle { get; set; }
        public bool IsManager { get; set; }
        public bool IsUnique { get; set; }
        public string Callsign { get; set; }
        public int LineupPosition { get; set; }
        public int ParentComponentId { get; set; }
        public string ParentComponentName { get; set; }
        public int MemberCount { get; set; }

    }
}
