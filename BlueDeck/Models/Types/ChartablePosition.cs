using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class ChartablePosition
    {
        public int positionId { get; set; }
        public string positionName { get; set; }
        public int parentComponentId { get; set; }
        public bool isUnique { get; set; }
    }
}
