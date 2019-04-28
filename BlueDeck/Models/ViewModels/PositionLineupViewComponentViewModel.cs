using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class PositionLineupViewComponentViewModel
    {
        [Display(Name = "Current Positions")]
        public List<PositionLineupItem> Positions { get; set; }
        public Position PositionToEdit { get; set; }

        public PositionLineupViewComponentViewModel(List<PositionLineupItem> positions, Position positionBeingEdited = null)
        {
            Positions = positions.OrderBy(x => x.LineupPosition).ToList();
            if (positionBeingEdited == null)
            {
                PositionToEdit = new Position();
            }
            else
            {
                PositionToEdit = positionBeingEdited;
            }
        }
    }
}
