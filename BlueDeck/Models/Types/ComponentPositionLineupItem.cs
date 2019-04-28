using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class ComponentPositionLineupItem
    {
        public int ComponentId { get; set; }
        public int? LineupPosition { get; set; }
        public string ComponentName { get; set; }

        public ComponentPositionLineupItem()
        {
        }

        public ComponentPositionLineupItem(Component c)
        {
            ComponentId = c.ComponentId;
            LineupPosition = c.LineupPosition;
            ComponentName = c.Name;
        }

    }
}
