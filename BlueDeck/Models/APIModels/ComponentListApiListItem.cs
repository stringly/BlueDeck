using BlueDeck.Models.Types;


namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents an API List item result for the <see cref="Component"/> entity.
    /// </summary>
    public class ComponentListApiListItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public int ComponentId { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentListApiListItem"/> class.
        /// </summary>
        public ComponentListApiListItem()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentListApiListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that accepts a <see cref="ComponentListApiListItem"/>
        /// </remarks>
        /// <param name="_c">The <see cref="ComponentListApiListItem"/></param>
        public ComponentListApiListItem(ComponentListApiListItem _c)
        {
            Name = _c.Name;
            ComponentId = _c.ComponentId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentListApiListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that converts a <see cref="ComponentSelectListItem"/> to a <see cref="ComponentListApiListItem"/>
        /// </remarks>
        /// <param name="_c">The <see cref="ComponentSelectListItem"/></param>
        public ComponentListApiListItem(ComponentSelectListItem _c)
        {
            Name = _c.ComponentName;
            ComponentId = _c.Id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentListApiListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that accepts a <see cref="Component"/> entity and creates a <see cref="ComponentListApiListItem"/>
        /// </remarks>
        /// <param name="_c">The <see cref="Component"/></param>
        public ComponentListApiListItem(Component _c)
        {
            Name = _c.Name;
            ComponentId = _c.ComponentId;
        }
    }
}
