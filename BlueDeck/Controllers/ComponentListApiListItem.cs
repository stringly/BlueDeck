using BlueDeck.Models.Types;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    public class ComponentListApiListItem
    {
        public string Name { get; set; }
        public int ComponentId { get; set; }

        public ComponentListApiListItem()
        {
        }

        public ComponentListApiListItem(ComponentListApiListItem _c)
        {
            Name = _c.Name;
            ComponentId = _c.ComponentId;
        }

        public ComponentListApiListItem(ComponentSelectListItem _c)
        {
            Name = _c.ComponentName;
            ComponentId = _c.Id;
        }

        public ComponentListApiListItem(Component _c)
        {
            Name = _c.Name;
            ComponentId = _c.ComponentId;
        }
    }
}
