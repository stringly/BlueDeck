using BlueDeck.Models.Types;

namespace BlueDeck.Models.APIModels
{
    public class PositionListAPIListItem
    {
        public int PositionId { get; set; }
        public string Name { get; set; }

        public PositionListAPIListItem()
        {
        }

        public PositionListAPIListItem(PositionSelectListItem _item)
        {
            PositionId = _item.PositionId;
            Name = _item.PositionName;
        }

        public PositionListAPIListItem(Position _p)
        {
            PositionId = _p.PositionId;
            Name = _p.Name;
        }
    }
}
