using BlueDeck.Models.Types;
using System.Collections.Generic;

namespace BlueDeck.Models.ViewModels
{
    public class AdminPositionIndexListViewModel
    {
        public IEnumerable<AdminPositionIndexViewModelListItem> Positions { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string PositionNameSort { get; set; }
        public string ParentComponentNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
                
        public AdminPositionIndexListViewModel()
        {
            Positions = new List<AdminPositionIndexViewModelListItem>();
        }

        public AdminPositionIndexListViewModel(List<Position> _positions)
        {
            Positions = _positions.ConvertAll(x => new AdminPositionIndexViewModelListItem(x));
        }
    }
}