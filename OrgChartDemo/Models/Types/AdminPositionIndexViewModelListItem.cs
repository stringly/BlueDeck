using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class AdminPositionIndexViewModelListItem
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string JobTitle { get; set; }
        public bool IsManager { get; set; }
        public bool IsUnique { get; set; }
        public string Callsign { get; set; }
        public int? LineupPosition { get; set; }
        public int ParentComponentId { get; set; }
        public string ParentComponentName { get; set; }
        public int MembersCount { get; set; }        

        public AdminPositionIndexViewModelListItem()
        {
        }

        public AdminPositionIndexViewModelListItem(Position _p)
        {
            PositionId = _p.PositionId;
            PositionName = _p.Name;
            JobTitle = _p.JobTitle;
            IsManager = _p.IsManager;
            IsUnique = _p.IsUnique;
            Callsign = _p.Callsign;
            LineupPosition = _p.LineupPosition;
            ParentComponentId = _p.ParentComponentId;
            ParentComponentName = _p.ParentComponent.Name;
            MembersCount = _p?.Members.Count() ?? 0;            
        }

    }
}
