using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// A type that represents the Name and Id of any entity. This is used to create select lists in views.
    /// <remarks>
    /// This was initially created for a <see cref="Component"/>, but can be used for any Name/Id combination.
    /// </remarks>
    /// </summary>
    public class ComponentSelectListItem
    {
        /// <summary>
        /// Gets or sets the display name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string ComponentName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the component.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentSelectListItem"/> class.
        /// </summary>
        public ComponentSelectListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentSelectListItem"/> class.
        /// </summary>
        /// <param name="_c">The Component.</param>
        public ComponentSelectListItem(Component _c)
        {
            ComponentName = _c.Name;
            Id = _c.ComponentId;
        }

    }
}
