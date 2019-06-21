using BlueDeck.Models;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Type that represents vehicle models for use in Drop-down lists
    /// </summary>
    public class VehicleModelSelectListItem
    {
        /// <summary>
        /// Gets or sets the vehicle model identifier.
        /// </summary>
        /// <value>
        /// The vehicle model identifier.
        /// </value>
        public int VehicleModelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the vehicle model.
        /// </summary>
        /// <value>
        /// The name of the vehicle model.
        /// </value>
        public string VehicleModelName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleModelSelectListItem"/> class.
        /// </summary>
        public VehicleModelSelectListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleModelSelectListItem"/> class.
        /// </summary>
        /// <param name="_v">The v.</param>
        public VehicleModelSelectListItem(VehicleModel _v)
        {
            VehicleModelId = _v.VehicleModelId;
            VehicleModelName = $"{_v.VehicleModelName} {_v.Manufacturer.VehicleManufacturerName}";
        }
    }
}
