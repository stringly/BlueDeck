﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class PositionLineupItem
    {
        public int PositionId { get; set; }
        public int? LineupPosition { get; set; }
        public string PositionName { get; set; }
        public bool IsManager { get; set; }
        public bool IsAssistantManager { get; set; }

        public PositionLineupItem()
        {
        }
        public PositionLineupItem(Position p)
        {
            PositionId = p.PositionId;
            LineupPosition = p.LineupPosition;
            PositionName = p.Name;
            IsManager = p.IsManager;
            IsAssistantManager = p.IsAssistantManager;
        }
    }
}
