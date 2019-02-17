using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class PositionLineupItem
    {
        public int PositionId { get; set; }
        public int? LineupPosition { get; set; }
        public string PositionName { get; set; }
        public bool IsManager { get; set; }

        public PositionLineupItem()
        {
        }
        public PositionLineupItem(Position p)
        {
            PositionId = p.PositionId;
            LineupPosition = p.LineupPosition;
            PositionName = p.Name;
            IsManager = p.IsManager;
        }
    }
}
